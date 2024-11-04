using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface ICartItemService
    {
        Task<ReadCartItemDTO> CreateCartItemAsync(CreateUpdateCartItemDTO cartItem);
        Task DeleteCartItemAsync(int id);
        Task<ReadCartItemDTO> GetCartItemByIdAsync(int id);
        Task<IEnumerable<ReadCartItemDTO>> GetCartItemsAsync();
        Task<ReadCartItemDTO> UpdateCartItemAsync(int id, CreateUpdateCartItemDTO cartItem);
        Task<bool> EntityExistsAsync(int OrderId, int productId);
    }
}
