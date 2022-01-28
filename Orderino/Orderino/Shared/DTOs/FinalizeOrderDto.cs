namespace Orderino.Shared.DTOs
{
    public class FinalizeOrderDto
    {
        public string UserId { get; set; }

        public string OrderId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
