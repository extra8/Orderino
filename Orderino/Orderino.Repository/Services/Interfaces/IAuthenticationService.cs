using Orderino.Shared.DTOs;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginInfo> Authenticate(AuthDto authDto);
    }
}
