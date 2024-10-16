using System.Text.Json.Serialization;

namespace E_commerceClassLibrary.Models.Sales
{
    public class Order
    {
        private decimal _totalPrice;

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("TotalPrice cannot be negative.");
                }
                _totalPrice = value;
            }
        }

        //[JsonIgnore]
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public List<OrderStaffAssignment>? OrderStaffAssignments { get; set; } = new List<OrderStaffAssignment>();
    }
}
