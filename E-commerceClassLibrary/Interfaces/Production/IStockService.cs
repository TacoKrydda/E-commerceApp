using E_commerceClassLibrary.DTO.Production;

namespace E_commerceClassLibrary.Interfaces.Production
{
    public interface IStockService
    {
        Task<StockDTO> CreateStockAsync(CreateUpdateStockDTO stock);
        Task DeleteStockAsync(int id);
        Task<StockDTO> GetStockByIdAsync(int id);
        Task<IEnumerable<StockDTO>> GetStocksAsync();
        Task<StockDTO> UpdateStockAsync(int id, CreateUpdateStockDTO stock);
        Task<bool> EntityExistsAsync(int productId);
    }
}
