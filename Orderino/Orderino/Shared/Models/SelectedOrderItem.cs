namespace Orderino.Shared.Models
{
    public class SelectedOrderItem
    {
        public OrderItem OrderItem { get; set; }

        public string SelectorId { get; set; }

        public string SelectorFirstName { get; set; }

        public string SelectorLastName { get; set; }
    }
}
