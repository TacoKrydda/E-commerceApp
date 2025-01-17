﻿using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(CreateUpdateProductDTO product);
        Task DeleteProductAsync(int id);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsAsync(string? color, string? size, string? category, string? brand);
        Task<ProductDTO> UpdateProductAsync(int id, CreateUpdateProductDTO product);
        Task<IEnumerable<ProductDTO>> GetProductByNameAsync(string name);
        Task<bool> EntityExistsAsync(string name, int brandId, int colorId, int sizeId);
    }
}
