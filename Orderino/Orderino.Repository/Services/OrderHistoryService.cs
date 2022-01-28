using Newtonsoft.Json;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

using Orderino.Infrastructure.Services.Interfaces;

namespace Orderino.Infrastructure.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly Repository<Order> orderRepository;
        private readonly Repository<User> userRepository;
        private readonly Repository<FinalizedOrder> finalizedOrderRepository;

        public OrderHistoryService(Repository<Order> orderRepository, Repository<User> userRepository, Repository<FinalizedOrder> finalizedOrderRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.finalizedOrderRepository = finalizedOrderRepository;
        }

        public async Task Reorder(ReorderDto reoderDto)
        {
            User user = await userRepository.QueryItemAsync(reoderDto.UserId);
            if (user == null)
                return;

            FinalizedOrder finalizedOrderCollection = await finalizedOrderRepository.QueryItemAsync(reoderDto.OrderId);
            if (finalizedOrderCollection == null || finalizedOrderCollection.Orders == null)
                return;

            Order orderToReOrder = finalizedOrderCollection.Orders.FirstOrDefault(x => x.FinalizedId == reoderDto.FinalizedOrderId);
            if (orderToReOrder == null)
                return;

            string serialized = JsonConvert.SerializeObject(orderToReOrder);
            Order newOrder = JsonConvert.DeserializeObject<Order>(serialized);
            newOrder.Initiator = user;
            newOrder.Initiator.Favorites = null;
            newOrder.FinalizedId = null;
            newOrder.FinalizedTime = null;

            await orderRepository.Upsert(newOrder);
        }
    }
}
