using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly Repository<Order> orderRepository;
        private readonly Repository<Restaurant> restaurantRepository;
        private readonly Repository<User> userRepository;
        private readonly Repository<FinalizedOrder> finalizedOrderRepository;
        private readonly Repository<RestaurantStatistic> restaurantStatisticRepository;

        public CartService(Repository<Order> orderRepository, Repository<Restaurant> restaurantRepository, Repository<User> userRepository, Repository<FinalizedOrder> finalizedOrderRepository, Repository<RestaurantStatistic> restaurantStatisticRepository)
        {
            this.orderRepository = orderRepository;
            this.restaurantRepository = restaurantRepository;
            this.userRepository = userRepository;
            this.finalizedOrderRepository = finalizedOrderRepository;
            this.restaurantStatisticRepository = restaurantStatisticRepository;
        }

        public async Task AddToCart(CartDto cartDto)
        {
            if (cartDto.Quantity <= 0)
                return;
            
            Restaurant restaurant = await restaurantRepository.QueryItemAsync(cartDto.RestaurantId);
            if (restaurant == null)
                return;

            OrderItem orderItem = restaurant.Menu.FirstOrDefault(x => x.Id == cartDto.OrderItemId);
            if (orderItem == null)
                return;

            Order order = await orderRepository.QueryItemAsync(cartDto.OrderId);
            if (order == null)
                return;

            User user = await userRepository.QueryItemAsync(cartDto.UserId);
            if (user == null)
                return;

            if (order.Items == null)
                order.Items = new List<SelectedOrderItem>();

            SelectedOrderItem itemInOrderAlready = order.Items.FirstOrDefault(x => x.OrderItem.Id == cartDto.OrderItemId && x.SelectorId == user.Id);

            if (itemInOrderAlready != null)
            {
                itemInOrderAlready.OrderItem.Quantity += cartDto.Quantity;
            }
            else
            {
                orderItem.Quantity = cartDto.Quantity;

                order.Items.Add(new SelectedOrderItem
                {
                    OrderItem = orderItem,
                    SelectorId = user.Id,
                    SelectorFirstName = user.FirstName,
                    SelectorLastName = user.LastName
                });
            }

            order.Total = order.Items.Select(x => x.OrderItem.Price * x.OrderItem.Quantity).ToList().Sum();
            await orderRepository.Update(order);
        }

        public async Task RemoveFromCart(CartDto cartDto)
        {
            Order order = await orderRepository.QueryItemAsync(cartDto.OrderId);
            if (order == null)
                return;

            if (string.IsNullOrEmpty(cartDto.SelectorUserId))
                return;

            if (cartDto.UserId != cartDto.SelectorUserId || cartDto.UserId != order.Initiator.Id)
                return;

            order.Items.RemoveAll(x => x.SelectorId == cartDto.SelectorUserId && x.OrderItem.Id == cartDto.OrderItemId);
            await orderRepository.Update(order);
        }

        public async Task ModifyOrder(CartDto cartDto)
        {
            Order order = await orderRepository.QueryItemAsync(cartDto.OrderId);
            if (order == null)
                return;

            if (cartDto.UserId != cartDto.SelectorUserId || cartDto.UserId != order.Initiator.Id)
                return;

            SelectedOrderItem selectedOrderItem = order.Items.FirstOrDefault(x => x.SelectorId == cartDto.SelectorUserId && x.OrderItem.Id == cartDto.OrderItemId);

            if (cartDto.Quantity <= 0)
            {
                order.Items.Remove(selectedOrderItem);
            }
            else
            {
                selectedOrderItem.OrderItem.Quantity = cartDto.Quantity;
            }

            order.Total = order.Items.Select(x => x.OrderItem.Price * x.OrderItem.Quantity).ToList().Sum();
            await orderRepository.Update(order);
        }

        public async Task FinalizeOrder(FinalizeOrderDto finalizeOrderDto)
        {
            Order order = await orderRepository.QueryItemAsync(finalizeOrderDto.OrderId);
            if (order == null)
                return;

            if (finalizeOrderDto.UserId != order.Initiator.Id)
                return;

            await FinalizeOrder(order, finalizeOrderDto);
            await InsertStatistics(order);

            await orderRepository.Delete(order);
        }        

        private async Task FinalizeOrder(Order order, FinalizeOrderDto finalizeOrderDto)
        {
            FinalizedOrder finalizedOrdersCollection = await finalizedOrderRepository.QueryItemAsync(order.Id, true);
            FillInFinalizedFields(order, finalizedOrdersCollection, finalizeOrderDto);
            await finalizedOrderRepository.Update(finalizedOrdersCollection);
        }

        private void FillInFinalizedFields(Order order, FinalizedOrder finalizedOrdersCollection, FinalizeOrderDto finalizeOrderDto)
        {
            if (finalizedOrdersCollection.Orders == null)
                finalizedOrdersCollection.Orders = new List<Order>();

            finalizedOrdersCollection.Id = order.Id;
            order.FinalizedId = Guid.NewGuid().ToString();
            order.FinalizedTime = DateTime.UtcNow;
            order.DeliveryAddress = new DeliveryAddress
            {
                FirstName = finalizeOrderDto.FirstName,
                LastName = finalizeOrderDto.LastName,
                Address = finalizeOrderDto.Address,
                Phone = finalizeOrderDto.Phone,
                AditionalInfo = finalizeOrderDto.AdditionalInfo
            };

            finalizedOrdersCollection.Orders.Add(order);            
        }

        private async Task InsertStatistics(Order order)
        {
            List<string> restaurantIds = order.Items.Select(x => x.OrderItem.RestaurantId).Distinct().ToList();
            List<RestaurantStatistic> restaurantStatistics = await restaurantStatisticRepository.QueryAllItemsAsync(restaurantIds);
            List<Restaurant> restaurants = await restaurantRepository.QueryAllItemsAsync(restaurantIds);

            foreach (string id in restaurantIds)
            {
                RestaurantStatistic statistic = restaurantStatistics.FirstOrDefault(x => x.Id == id);

                if (statistic == null)
                {
                    Restaurant restaurant = restaurants.FirstOrDefault(x => x.Id == id);

                    RestaurantStatistic newStatistic = new RestaurantStatistic
                    {
                        Id = id,
                        HistoricalMenu = new List<OrderItem>(),
                        History = new List<StatisticData>(),
                        Name = restaurant.Name,
                        Address = restaurant.Address,
                        Phone = restaurant.Phone,
                        Email = restaurant.Email
                    };

                    restaurantStatistics.Add(newStatistic);
                }
            }

            FillInStatistics(order, restaurantStatistics);
            await restaurantStatisticRepository.BulkAddAsync(restaurantStatistics);
        }

        private void FillInStatistics(Order order, List<RestaurantStatistic> restaurantStatistics)
        {
            DateTime now = DateTime.UtcNow;
            foreach (SelectedOrderItem item in order.Items)
            {
                RestaurantStatistic statistic = restaurantStatistics.FirstOrDefault(x => x.Id == item.OrderItem.RestaurantId);

                if (statistic.HistoricalMenu == null)
                    statistic.HistoricalMenu = new List<OrderItem>();

                if (statistic.History == null)
                    statistic.History = new List<StatisticData>();

                if (statistic.HistoricalMenu.Count(x => x.Id == item.OrderItem.Id) == 0)
                    statistic.HistoricalMenu.Add(item.OrderItem);

                statistic.History.Add(new StatisticData { OrderItemId = item.OrderItem.Id, TimeStamp = now, Quantity = item.OrderItem.Quantity });
            }
        }
    }
}
