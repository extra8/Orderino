using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto authDto)
        {
            LoginInfo loginInfo = await authenticationService.Authenticate(authDto);
            return Ok(loginInfo);
        }
    }
}
