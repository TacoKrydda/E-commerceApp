using System.Text.Json.Serialization;

namespace E_commerceClassLibrary.Models.Production
{
    public class Stock
    {
        private int _quantity;

        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Quantity cannot be negative.");
                }
                _quantity = value;
            }
        }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
