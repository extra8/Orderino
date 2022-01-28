using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("restaurant")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [HttpGet("{restaurantId:required}")]
        public async Task<IActionResult> Get(string restaurantId)
        {
            Restaurant result = await restaurantService.Get(restaurantId);
            return Ok(result);
        }

        [HttpGet("all/{search}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(string search = null)
        {
            List<Restaurant> result = await restaurantService.GetAll(search);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Update([FromBody] Restaurant modifiedRestaurant)
        {
            await restaurantService.Update(modifiedRestaurant);
            return Ok();
        }
    }
}
