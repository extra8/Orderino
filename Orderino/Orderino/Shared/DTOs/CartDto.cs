namespace Orderino.Shared.DTOs
{
    public class CartDto
    {
        public string UserId { get; set; }

        public string SelectorUserId { get; set; }

        public string OrderId { get; set; }

        public string RestaurantId { get; set; }

        public string OrderItemId { get; set; }

        public int Quantity { get; set; }
    }
}
