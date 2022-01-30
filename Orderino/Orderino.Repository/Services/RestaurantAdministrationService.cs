using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System;
using Orderino.Infrastructure.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services
{
    public class RestaurantAdministrationService : IRestaurantAdministrationService
    {
        private readonly Repository<LoginInfo> loginInfoRepository;
        private readonly Repository<Restaurant> restaurantRepository;

        public RestaurantAdministrationService(Repository<LoginInfo> loginInfoRepository, Repository<Restaurant> restaurantRepository)
        {
            this.loginInfoRepository = loginInfoRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Restaurant> EditRestaurant(ModifyRestaurantDto restaurantDto)
        {
            if (restaurantDto.Restaurant.Menu.Any(x => string.IsNullOrEmpty(x.MenuCategory)))
                return null;

            if (restaurantDto.Restaurant.Menu.Any(x => string.IsNullOrEmpty(x.Name)))
                return null;

            LoginInfo loginInfo = (await loginInfoRepository.QueryByFieldName("Token", restaurantDto.Token)).FirstOrDefault();

            bool hasAccessToRestaurant = loginInfo.Restaurants.Select(x => x.RestaurantId).Contains(restaurantDto.Restaurant.Id);

            if (loginInfo == null || loginInfo.TokenExpiration < DateTime.UtcNow || !hasAccessToRestaurant)
                return null;

            Restaurant restaurant = await restaurantRepository.QueryItemAsync(restaurantDto.Restaurant.Id);
            if (restaurant == null)
                return null;

            if (restaurant.Name != restaurantDto.Restaurant.Name)
            { 
                restaurantDto.Restaurant.Menu.ForEach(x => x.RestaurantName = restaurantDto.Restaurant.Name);
            }
            else
            {
                restaurantDto.Restaurant.Menu.ForEach(x => x.RestaurantName = restaurant.Name);
            }

            restaurantDto.Restaurant.Menu.RemoveAll(x => string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.MenuCategory));

            loginInfo.Restaurants.First(x => x.RestaurantId == restaurant.Id).RestaurantName = restaurantDto.Restaurant.Name;
            loginInfo.Restaurants.First(x => x.RestaurantId == restaurant.Id).BannerUrl = restaurantDto.Restaurant.BannerUrl;

            restaurant = restaurantDto.Restaurant;
            restaurant.MenuCategories.RemoveAll(x => string.IsNullOrEmpty(x));

            await restaurantRepository.Update(restaurant);
            await loginInfoRepository.Update(loginInfo);

            return restaurant;
        }
    }
}
