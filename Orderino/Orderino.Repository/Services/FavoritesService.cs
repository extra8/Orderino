using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly Repository<User> userRepository;
        private readonly Repository<Restaurant> restaurantRepository;

        public FavoritesService(Repository<User> userRepository, Repository<Restaurant> restaurantRepository)
        {
            this.userRepository = userRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task AddToFavorites(FavoriteDto favoriteDto)
        {
            User user = await userRepository.QueryItemAsync(favoriteDto.UserId);
            if (user == null)
                return;

            Restaurant restaurant = await restaurantRepository.QueryItemAsync(favoriteDto.RestaurantId);
            if (restaurant == null)
                return;            

            if (user.Favorites == null)
                user.Favorites = new List<OrderItem>();

            OrderItem product = restaurant.Menu.FirstOrDefault(x => x.Id == favoriteDto.OrderItemId);
            bool userHasProduct = user.Favorites.Any(x => x.Id == favoriteDto.OrderItemId);

            if (!userHasProduct && product != null)
            {
                user.Favorites.Add(product);
                await userRepository.Update(user);
            }
        }

        public async Task RemoveFromFavorites(FavoriteDto favoriteDto)
        {
            User user = await userRepository.QueryItemAsync(favoriteDto.UserId);

            if (user?.Favorites != null)
            {
                bool userHasProduct = user.Favorites.Any(x => x.Id == favoriteDto.OrderItemId);

                if (userHasProduct)
                {
                    user.Favorites.RemoveAll(x => x.Id == favoriteDto.OrderItemId);
                    await userRepository.Update(user);
                }
            }                   
        }
    }
}
