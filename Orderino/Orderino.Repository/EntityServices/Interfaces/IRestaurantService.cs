using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface IRestaurantService
    {
        Task<Restaurant> Get(string restaurantId);

        Task Update(Restaurant modifiedRestaurant);

        Task<List<Restaurant>> GetAll(string search = null);
    }
}
