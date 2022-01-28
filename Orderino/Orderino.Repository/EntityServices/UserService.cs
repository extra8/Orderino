using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class UserService : IUserService
    {
        private readonly Repository<User> userRepository;

        public UserService(Repository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> Get(string userId)
        {
            return await userRepository.QueryItemAsync(userId);
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
    }
}
