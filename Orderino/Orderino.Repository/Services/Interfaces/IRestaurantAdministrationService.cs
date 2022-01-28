using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services.Interfaces
{
    public interface IRestaurantAdministrationService
    {
        Task<Restaurant> EditRestaurant(ModifyRestaurantDto restaurantDto);
    }
}
