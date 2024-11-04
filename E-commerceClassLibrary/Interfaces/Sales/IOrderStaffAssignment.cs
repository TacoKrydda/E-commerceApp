using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface IOrderStaffAssignment
    {
        Task<ReadOrderStaffAssignmentDTO> CreateOrderStaffAssignmentAsync(CreateUpdateOrderStaffAssignmentDTO assignment);
        Task DeleteOrderStaffAssignmentAsync(int id);
        Task<ReadOrderStaffAssignmentDTO> GetOrderStaffAssignmentByIdAsync(int id);
        Task<IEnumerable<ReadOrderStaffAssignmentDTO>> GetOrderStaffAssignmentsAsync();
        Task<ReadOrderStaffAssignmentDTO> UpdateOrderStaffAssignmentAsync(int id, CreateUpdateOrderStaffAssignmentDTO assignment);
        Task<bool> EntityExistsAsync(int staffId);
    }
}
