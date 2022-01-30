using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{userId:required}")]
        public async Task<IActionResult> Get(string userId)
        {
            User result = await userService.Get(userId);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Update([FromBody] User modifiedUser)
        {
            await userService.Update(modifiedUser);
            return Ok();
        }

        [HttpPost("recent")]
        public async Task<IActionResult> AddToRecent([FromBody] RecentDto recentDto)
        {
            await userService.AddToRecent(recentDto);
            return Ok();
        }
    }
}
