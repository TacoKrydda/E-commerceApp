namespace E_commerceClassLibrary.DTO.Sales
{
    public class CreateUpdateCartItemDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
