using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface IColorService
    {
        Task<ColorDTO> CreateColorAsync(ColorDTO color);
        Task DeleteColorAsync(int id);
        Task<ColorDTO> GetColorByIdAsync(int id);
        Task<IEnumerable<ColorDTO>> GetColorsAsync();
        Task<ColorDTO> UpdateColorAsync(int id, ColorDTO color);
        Task<bool> EntityExistsAsync(string name);
    }
}
