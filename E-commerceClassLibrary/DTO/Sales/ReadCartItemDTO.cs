﻿namespace E_commerceClassLibrary.DTO.Sales
{
    public class ReadCartItemDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}