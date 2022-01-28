using Microsoft.AspNetCore.Mvc;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Server.Controllers
{
    [Route("favorites")]
    [ApiController]
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDto favoriteDto)
        {
            await favoritesService.AddToFavorites(favoriteDto);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteDto favoriteDto)
        {
            await favoritesService.RemoveFromFavorites(favoriteDto);
            return Ok();
        }
    }
}
