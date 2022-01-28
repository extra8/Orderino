using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Shared.Models;
using System.Threading.Tasks;

namespace Orderino.Infrastructure.EntityServices
{
    public class FinalizedOrderService : IFinalizedOrderService
    {
        private readonly Repository<FinalizedOrder> finalizedOrderRepository;

        public FinalizedOrderService(Repository<FinalizedOrder> finalizedOrderRepository)
        {
            this.finalizedOrderRepository = finalizedOrderRepository;
        }

        public async Task<FinalizedOrder> Get(string id)
        {
            return await finalizedOrderRepository.QueryItemAsync(id, true);
        }

        public async Task Update(FinalizedOrder modifiedFinalizedOrder)
        {
            if (modifiedFinalizedOrder == null)
                return;

            FinalizedOrder finalizedOrder = await finalizedOrderRepository.QueryItemAsync(modifiedFinalizedOrder.Id);
            if (finalizedOrder == null)
                return;

            await finalizedOrderRepository.Update(modifiedFinalizedOrder);
        }
    }
}
