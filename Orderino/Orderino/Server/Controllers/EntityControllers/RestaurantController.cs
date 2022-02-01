using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.DTOs;
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

        [HttpPost("all")]
        public async Task<IActionResult> GetAll([FromBody] SearchDto searchDto)
        {
            List<Restaurant> result = await restaurantService.GetAll(searchDto.Search);
            return Ok(result);
        }

        [HttpPost("all-basic")]
        public async Task<IActionResult> GetAllBasic([FromBody] SearchDto searchDto)
        {
            List<Restaurant> result = await restaurantService.GetAllBasic(searchDto.Search);
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
