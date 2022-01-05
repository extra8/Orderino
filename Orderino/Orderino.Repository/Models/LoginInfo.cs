using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class LoginInfo : IEntity
    {
        public string Id { get; set; }

        public List<RestaurantAdmin> Administrators { get; set; }
    }
}
