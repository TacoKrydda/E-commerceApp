using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface ICustomerService
    {
        Task<ReadCustomerDTO> CreateCustomerAsync(CreateUpdateCustomerDTO customer);
        Task DeleteCustomerAsync(int id);
        Task<ReadCustomerDTO> GetCustomerByIdAsync(int id);
        Task<IEnumerable<ReadCustomerDTO>> GetCustomersAsync();
        Task<ReadCustomerDTO> UpdateCustomerAsync(int id, CreateUpdateCustomerDTO customer);
        Task<bool> EntityExistsAsync(string email);
    }
}
