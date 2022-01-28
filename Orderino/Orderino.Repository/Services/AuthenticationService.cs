using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Repository<LoginInfo> loginInfoRepository;

        public AuthenticationService(Repository<LoginInfo> loginInfoRepository)
        {
            this.loginInfoRepository = loginInfoRepository;
        }

        public async Task<LoginInfo> Authenticate(AuthDto authDto)
        {
            if (string.IsNullOrWhiteSpace(authDto.Username) || string.IsNullOrWhiteSpace(authDto.Password))
                return null;

            LoginInfo loginInfo = await loginInfoRepository.QueryItemAsync(authDto.Username);
            if (loginInfo == null)
                return null;

            if (loginInfo.Password != authDto.Password)
                return null;

            loginInfo.Token = Guid.NewGuid().ToString();
            loginInfo.TokenExpiration = DateTime.UtcNow.AddDays(1);

            await loginInfoRepository.Update(loginInfo);

            return loginInfo;
        }
    }
}
