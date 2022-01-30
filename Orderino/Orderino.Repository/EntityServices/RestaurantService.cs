using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{

    public class RestaurantService : IRestaurantService
    {
        private readonly Repository<Restaurant> restaurantRepository;

        public RestaurantService(Repository<Restaurant> restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Restaurant> Get(string restaurantId)
        {
            return await restaurantRepository.QueryItemAsync(restaurantId);
        }

        public async Task<List<Restaurant>> GetAll(string search = null)
        {
            if (!string.IsNullOrEmpty(search))
                return await restaurantRepository.QueryByFieldName("Name", search);

            return await restaurantRepository.QueryAllItemsAsync();
        }

        public async Task<List<Restaurant>> GetAllBasic(string search = null)
        {
            List<Restaurant> restaurants;

            if (!string.IsNullOrEmpty(search))
                restaurants = await restaurantRepository.QueryByFieldName("Name", search);

            restaurants = await restaurantRepository.QueryAllItemsAsync();

            restaurants.ForEach(x => x.Menu = null);

            return restaurants;
        }

        public async Task Update(Restaurant modifiedRestaurant)
        {
            if (modifiedRestaurant == null)
                return;

            Restaurant restaurant = await restaurantRepository.QueryItemAsync(modifiedRestaurant.Id);
            if (restaurant == null)
                return;

            await restaurantRepository.Update(modifiedRestaurant);
        }
    }
}
