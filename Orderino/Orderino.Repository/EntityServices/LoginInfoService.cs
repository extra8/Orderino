using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class LoginInfoService : ILoginInfoService
    {
        private readonly Repository<LoginInfo> loginInfoRepository;

        public LoginInfoService(Repository<LoginInfo> loginInfoRepository)
        {
            this.loginInfoRepository = loginInfoRepository;
        }

        public async Task<LoginInfo> Get(string token)
        {
            LoginInfo loginInfo = (await loginInfoRepository.QueryByFieldName("Token", token)).FirstOrDefault();

            if (loginInfo != null && loginInfo.TokenExpiration >= DateTime.UtcNow)
            {
                return loginInfo;
            }

            return null;
        }
    }
}
