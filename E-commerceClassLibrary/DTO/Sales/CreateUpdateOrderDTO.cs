using E_commerceClassLibrary.Models.Sales;

namespace E_commerceClassLibrary.DTO.Sales
{
    public class CreateUpdateOrderDTO
    {
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
