using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices.Interfaces
{
    public interface IFinalizedOrderService
    {
        Task<FinalizedOrder> Get(string id);

        Task Update(FinalizedOrder modifiedFinalizedOrder);
    }
}
