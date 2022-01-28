using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface ILoginInfoService
    {
        Task<LoginInfo> Get(string token);
    }
}
