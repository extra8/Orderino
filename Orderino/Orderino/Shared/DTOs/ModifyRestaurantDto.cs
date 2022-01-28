using Orderino.Shared.Models;

namespace Orderino.Shared.DTOs
{
    public class ModifyRestaurantDto
    {
        public string Token { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
