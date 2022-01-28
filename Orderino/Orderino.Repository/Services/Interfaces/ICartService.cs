using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(CartDto cartDto);

        Task RemoveFromCart(CartDto cartDto);

        Task ModifyOrder(CartDto cartDto);

        Task FinalizeOrder(FinalizeOrderDto finalizeOrderDto);
    }
}
