using Orderino.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace Orderino.Shared.Models
{
    public class Order : IEntity
    {
        public string Id { get; set; }

        public List<SelectedOrderItem> Items { get; set; }

        public double Total { get; set; }

        public DeliveryAddress DeliveryAddress { get; set; }

        public User Initiator { get; set; }

        public string FinalizedId { get; set; }

        public DateTime? FinalizedTime { get; set; }

        public OrderType OrderType { get; set; }

        public List<string> DoneUserIds { get; set; }
    }
}
