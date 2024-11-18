namespace E_commerceClassLibrary.DTO.Production
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<ColorDTO>? ColorOptions { get; set; }
    }
}
