using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface IUserService
    {
        Task<User> Get(string userId);

        Task Update(User modifiedUser);
    }
}
