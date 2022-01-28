using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("finalized-orders")]
    [ApiController]
    public class FinalizedOrderController : Controller
    {
        private readonly IFinalizedOrderService finalizedOrderService;

        public FinalizedOrderController(IFinalizedOrderService finalizedOrderService)
        {
            this.finalizedOrderService = finalizedOrderService;
        }

        [HttpGet("{id:required}")]
        public async Task<IActionResult> Get(string id)
        {
            FinalizedOrder result = await finalizedOrderService.Get(id);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Update([FromBody] FinalizedOrder modifiedFinalizedOrder)
        {
            await finalizedOrderService.Update(modifiedFinalizedOrder);
            return Ok();
        }
    }
}
