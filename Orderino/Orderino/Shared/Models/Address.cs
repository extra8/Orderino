namespace Orderino.Shared.Models
{
    public class Address
    {
        public string City { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public override string ToString()
        {
            return $"{Line1}, {Line2}, {City}";
        }
    }
}
