using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    [Route("")]
    [ApiController]
    public class RestaurantAdministrationController : Controller
    {
        private readonly IRestaurantAdministrationService restaurantAdministrationService;

        public RestaurantAdministrationController(IRestaurantAdministrationService restaurantAdministrationService)
        {
            this.restaurantAdministrationService = restaurantAdministrationService;
        }

        [HttpPost("restaurant-edit")]
        public async Task<IActionResult> EditRestaurant([FromBody] ModifyRestaurantDto restaurantDto)
        {
            Restaurant result =  await restaurantAdministrationService.EditRestaurant(restaurantDto);
            return Ok(result);
        }
    }
}
