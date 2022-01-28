using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    [Route("cart")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartDto cartDto)
        {
            await cartService.AddToCart(cartDto);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartDto cartDto)
        {
            await cartService.RemoveFromCart(cartDto);
            return Ok();
        }

        [HttpPost("modify")]
        public async Task<IActionResult> ModifyOrder([FromBody] CartDto cartDto)
        {
            await cartService.ModifyOrder(cartDto);
            return Ok();
        }

        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizeOrder([FromBody] FinalizeOrderDto orderDto)
        {
            await cartService.FinalizeOrder(orderDto);
            return Ok();
        }
    }
}
