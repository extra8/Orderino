using System;

namespace Orderino.Repository.Models
{
    public class RestaurantAdmin
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}
