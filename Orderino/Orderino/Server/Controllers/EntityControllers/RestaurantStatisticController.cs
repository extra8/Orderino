using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("statistics")]
    [ApiController]
    public class RestaurantStatisticController : Controller
    {
        private readonly IRestaurantStatisticService restaurantStatisticService;

        public RestaurantStatisticController(IRestaurantStatisticService restaurantStatisticService)
        {
            this.restaurantStatisticService = restaurantStatisticService;
        }        

        [HttpGet("{id:required}")]
        public async Task<IActionResult> Get(string id)
        {
            RestaurantStatistic result = await restaurantStatisticService.Get(id);
            return Ok(result);
        }
    }
}
