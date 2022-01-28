using System;
using System.Collections.Generic;

namespace Orderino.Shared.Models
{
    public class LoginInfo : IEntity
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public List<RestaurantAdministration> Restaurants { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}
