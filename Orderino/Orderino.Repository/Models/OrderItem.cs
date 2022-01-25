namespace Orderino.Repository.Models
{
    public class OrderItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string MenuCategory { get; set; }

        public string RestaurantName { get; set; }

        public string RestaurantId { get; set; }

        public string Description { get; set; }

        public string ImageData { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }        
    }
}
