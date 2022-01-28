using System;
using System.Collections.Generic;

namespace Orderino.Shared.Models
{
    public class RestaurantStatistic : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        public List<OrderItem> HistoricalMenu { get; set; }

        public List<StatisticData> History { get; set; }
    }
}
