using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class OrderService : IOrderService
    {
        private readonly Repository<Order> orderRepository;

        public OrderService(Repository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Order> Get(string orderId)
        {
            return await orderRepository.QueryItemAsync(orderId, true);  ///////////// scos true
        }

        public async Task Update(Order modifiedOrder)
        {
            if (modifiedOrder == null)
                return;

            Order order = await orderRepository.QueryItemAsync(modifiedOrder.Id);
            if (order == null)
                return;

            await orderRepository.Update(modifiedOrder);
        }
    }
}
