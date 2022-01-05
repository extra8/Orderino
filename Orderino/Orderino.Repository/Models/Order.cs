using System.Collections.Generic;

namespace Orderino.Repository.Models
{
    public class Order : IEntity
    {
        public string Id { get; set; }

        public List<SelectedOrderItem> Items { get; set; }

        public double Total { get; set; }

        public Address DeliveryAddress { get; set; }

        public string DeliveryPhone { get; set; }

        public User Initiator { get; set; }
    }
}
