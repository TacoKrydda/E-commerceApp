using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface IStaffService
    {
        Task<ReadStaffDTO> CreateStaffAsync(CreateUpdateStaffDTO staff);
        Task DeleteStaffAsync(int id);
        Task<ReadStaffDTO> GetStaffByIdAsync(int id);
        Task<IEnumerable<ReadStaffDTO>> GetStaffsAsync();
        Task<ReadStaffDTO> UpdateStaffAsync(int id, CreateUpdateStaffDTO staff);
        Task<bool> EntityExistsAsync(string email);
    }
}
