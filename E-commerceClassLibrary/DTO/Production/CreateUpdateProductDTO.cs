namespace E_commerceClassLibrary.DTO.Production
{
    public class CreateUpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
