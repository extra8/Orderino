using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class RestaurantStatisticService : IRestaurantStatisticService
    {
        private readonly Repository<RestaurantStatistic> restaurantStatisticRepository;

        public RestaurantStatisticService(Repository<RestaurantStatistic> restaurantStatisticRepository)
        {
            this.restaurantStatisticRepository = restaurantStatisticRepository;
        }

        public async Task<RestaurantStatistic> Get(string id)
        {
            return await restaurantStatisticRepository.QueryItemAsync(id);
        }
    }
}
