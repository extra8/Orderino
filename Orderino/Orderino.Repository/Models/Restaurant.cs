using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class Restaurant : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        public List<OrderItem> Menu { get; set; }

        public string Description { get; set; }

        public string BannerString { get; set; }
    }
}
