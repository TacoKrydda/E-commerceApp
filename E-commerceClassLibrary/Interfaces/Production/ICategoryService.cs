using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
        Task DeleteCategoryAsync(int id);
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO category);
        Task<bool> EntityExistsAsync(string name);
    }
}
