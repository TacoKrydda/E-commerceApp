using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface ISizeService
    {
        Task<SizeDTO> CreateSizeAsync(SizeDTO size);
        Task DeleteSizeAsync(int id);
        Task<SizeDTO> GetSizeByIdAsync(int id);
        Task<IEnumerable<SizeDTO>> GetSizesAsync();
        Task<SizeDTO> UpdateSizeAsync(int id, SizeDTO size);
        Task<bool> EntityExistsAsync(string name);
    }
}
