using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System;
using System.Text;
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

            if (loginInfo.Password != Base64Encode(authDto.Password))
                return null;

            loginInfo.Token = Guid.NewGuid().ToString();
            loginInfo.TokenExpiration = DateTime.UtcNow.AddDays(1);

            await loginInfoRepository.Update(loginInfo);

            return loginInfo;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
