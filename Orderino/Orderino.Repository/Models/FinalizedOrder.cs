using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class FinalizedOrder : IEntity
    {
        public string Id { get; set; }

        public List<Order> Orders { get; set; }
    }
}
