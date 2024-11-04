namespace E_commerceClassLibrary.DTO.Sales
{
    public class ReadOrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ReadCartItemDTO>? CartItems { get; set; } = new List<ReadCartItemDTO>();
    }
}
