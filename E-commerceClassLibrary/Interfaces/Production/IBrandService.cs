using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface IBrandService
    {
        Task<BrandDTO> CreateBrandAsync(BrandDTO brand);
        Task DeleteBrandAsync(int id);
        Task<BrandDTO> GetBrandByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetBrandsAsync();
        Task<BrandDTO> UpdateBrandAsync(int id, BrandDTO brand);
        Task<bool> EntityExistsAsync(string name);
    }
}
