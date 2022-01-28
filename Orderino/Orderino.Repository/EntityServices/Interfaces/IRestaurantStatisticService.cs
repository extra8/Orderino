using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface IRestaurantStatisticService
    {
        Task<RestaurantStatistic> Get(string id);
    }
}
