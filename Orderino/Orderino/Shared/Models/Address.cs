namespace Orderino.Shared.Models
{
    public class Address
    {
        public string City { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public override string ToString()
        {
            string address = "";

            if (!string.IsNullOrEmpty(Line1))
                address = Line1;

            if (!string.IsNullOrEmpty(Line2))
                address += $", {Line2}";

            if (!string.IsNullOrEmpty(City))
                address += $", {City}";

            return address;
        }
    }
}
