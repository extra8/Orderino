using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{orderId:required}")]
        public async Task<IActionResult> Get(string orderId)
        {
            Order result = await orderService.Get(orderId); 
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Update([FromBody] Order modifiedOrder)
        {
            await orderService.Update(modifiedOrder);
            return Ok();
        }
    }
}
