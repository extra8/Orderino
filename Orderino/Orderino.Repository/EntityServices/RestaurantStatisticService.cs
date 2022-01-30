using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class RestaurantStatisticService : IRestaurantStatisticService
    {
        private readonly Repository<RestaurantStatistic> restaurantStatisticRepository;
        private readonly Repository<Restaurant> restaurantRepository;

        public RestaurantStatisticService(Repository<RestaurantStatistic> restaurantStatisticRepository, Repository<Restaurant> restaurantRepository)
        {
            this.restaurantStatisticRepository = restaurantStatisticRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<RestaurantStatistic> Get(string id)
        {
            RestaurantStatistic statistic = await restaurantStatisticRepository.QueryItemAsync(id);

            if (statistic == null)
            {
                Restaurant restaurant = await restaurantRepository.QueryItemAsync(id);

                statistic = new RestaurantStatistic
                {
                    Id = id,
                    Name = restaurant.Name,
                    Email = restaurant.Email,
                    Phone = restaurant.Phone,
                    Address = restaurant.Address,
                    HistoricalMenu = new List<OrderItem>(),
                    History = new List<StatisticData>()
                };

                await restaurantStatisticRepository.AddAsync(statistic);
            }

            return statistic;
        }
    }
}
