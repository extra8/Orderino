using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers.EntityControllers
{
    [Route("login")]
    [ApiController]
    public class LoginInfoController : Controller
    {
        private readonly ILoginInfoService loginInfoService;

        public LoginInfoController(ILoginInfoService loginInfoService)
        {
            this.loginInfoService = loginInfoService;
        }

        [HttpGet("{token:required}")]
        public async Task<IActionResult> Get(string token)
        {
            LoginInfo result = await loginInfoService.Get(token);
            return Ok(result);
        }
    }
}
