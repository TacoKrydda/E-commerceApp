using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customer);
        Task DeleteCustomerAsync(int id);
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerDTO>> GetCustomersAsync();
        Task<CustomerDTO> UpdateCustomerAsync(int id, CreateUpdateCustomer customer);
        Task<bool> EntityExistsAsync(string email);
    }
}
