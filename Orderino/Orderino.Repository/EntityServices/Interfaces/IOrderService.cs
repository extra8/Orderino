using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface IOrderService
    {
        Task<Order> Get(string orderId);

        Task Update(Order modifiedOrder);
    }
}
