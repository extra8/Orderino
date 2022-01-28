using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    [Route("")]
    [ApiController]
    public class OrderHistoryController : Controller
    {
        private readonly IOrderHistoryService orderHistoryService;

        public OrderHistoryController(IOrderHistoryService orderHistoryService)
        {
            this.orderHistoryService = orderHistoryService;
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] ReorderDto reoderDto)
        {
            await orderHistoryService.Reorder(reoderDto);
            return Ok();
        }
    }
}
