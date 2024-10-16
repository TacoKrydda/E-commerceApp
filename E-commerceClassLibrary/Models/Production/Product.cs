using System.Text.Json.Serialization;

namespace E_commerceClassLibrary.Models.Production
{
    public class Product
    {
        private decimal _price;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative.");
                }
                _price = value;
            }
        }

        [JsonIgnore]
        public Brand? Brand { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        [JsonIgnore]
        public Color? Color { get; set; }
        [JsonIgnore]
        public Size? Size { get; set; }
        [JsonIgnore]
        public Stock? Stock { get; set; }
    }
}
