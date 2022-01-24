using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class User : IEntity
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<OrderItem> Favorites { get; set; }

        public List<string> RecentRestaurantIds { get; set; }
    }
}
