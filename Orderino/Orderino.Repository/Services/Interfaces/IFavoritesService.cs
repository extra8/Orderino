using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services.Interfaces
{
    public interface IFavoritesService
    {
        Task AddToFavorites(FavoriteDto favoriteDto);

        Task RemoveFromFavorites(FavoriteDto favoriteDto);
    }
}
