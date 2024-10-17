using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(CreateUpdateProductDTO product);
        Task DeleteProductAsync(int id);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
        Task<ProductDTO> UpdateProductAsync(int id, CreateUpdateProductDTO product);
        Task<bool> EntityExistsAsync(string name, int brandId);
    }
}
