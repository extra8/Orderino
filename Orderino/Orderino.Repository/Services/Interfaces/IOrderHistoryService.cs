using Orderino.Shared.DTOs;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.Services.Interfaces
{
    public interface IOrderHistoryService
    {
        Task Reorder(ReorderDto reoderDto);
    }
}
