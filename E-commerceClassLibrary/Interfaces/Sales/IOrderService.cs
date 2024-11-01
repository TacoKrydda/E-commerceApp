using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface IOrderService
    {
        Task<ReadOrderDTO> CreateOrderAsync(CreateUpdateOrderDTO order);
        Task DeleteOrderAsync(int id);
        Task<ReadOrderDTO> GetOrderByIdAsync(int id);
        Task<IEnumerable<ReadOrderDTO>> GetOrdersAsync();
        Task<ReadOrderDTO> UpdateOrderAsync(int id, CreateUpdateOrderDTO order);
    }
}
