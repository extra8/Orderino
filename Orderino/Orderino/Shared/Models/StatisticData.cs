using System;

namespace Orderino.Shared.Models
{
    public class StatisticData
    {
        public string OrderItemId { get; set; }
         
        public DateTime TimeStamp { get; set; }
         
        public int Quantity { get; set; }
    }
}
