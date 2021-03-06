using System.Collections.Generic;

namespace Orderino.Shared.Models
{
    public class Restaurant : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Address Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string BannerUrl { get; set; }

        public List<string> MenuCategories { get; set; }

        public List<OrderItem> Menu { get; set; }        
    }
}
