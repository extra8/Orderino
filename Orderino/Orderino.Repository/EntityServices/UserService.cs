using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class UserService : IUserService
    {
        private readonly Repository<User> userRepository;
        private readonly Repository<Restaurant> restaurantRepository;

        public UserService(Repository<User> userRepository, Repository<Restaurant> restaurantRepository)
        {
            this.userRepository = userRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<User> Get(string userId)
        {
            return await userRepository.QueryItemAsync(userId, true);  ///////////// scos true
        }

        public async Task Update(User modifiedUser)
        {
            if (modifiedUser == null)
                return;

            User user = await userRepository.QueryItemAsync(modifiedUser.Id);
            if (user == null)
                return;

            await userRepository.Update(modifiedUser);
        }

        public async Task AddToRecent(RecentDto recentDto)
        {
            if (recentDto.UserId == null)
                return;

            if (recentDto.RestaurantId == null)
                return;

            User user = await userRepository.QueryItemAsync(recentDto.UserId);
            if (user == null)
                return;

            Restaurant restaurant = await restaurantRepository.QueryItemAsync(recentDto.RestaurantId);
            if (restaurant == null)
                return;

            if (user.RecentRestaurantIds == null)
                user.RecentRestaurantIds = new List<string>();

            if (user.RecentRestaurantIds.Contains(recentDto.RestaurantId))
                user.RecentRestaurantIds.Remove(recentDto.RestaurantId);

            user.RecentRestaurantIds.Add(recentDto.RestaurantId);

            if (user.RecentRestaurantIds.Count > 7)
            {
                int numberToRemove = user.RecentRestaurantIds.Count - 7;
                user.RecentRestaurantIds = user.RecentRestaurantIds.Skip(numberToRemove).Take(7).ToList();
            }

            await userRepository.Update(user);
        }
    }
}
