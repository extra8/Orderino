using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class Order : IEntity
    {
        public string Id { get; set; }

        public List<SelectedOrderItem> Items { get; set; }

        public double Total { get; set; }

        public string ResteaurantName { get; set; }

        public Address DeliveryAddress { get; set; }

        public Address RestaurantAddress { get; set; }

        public string DeliveryPhone { get; set; }

        public string RestaurantPhone { get; set; }

        public User Initiator { get; set; }
    }
}
