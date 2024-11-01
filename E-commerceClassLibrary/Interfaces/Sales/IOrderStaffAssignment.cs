using E_commerceClassLibrary.DTO.Sales;

namespace E_commerceClassLibrary.Interfaces.Sales
{
    public interface IOrderStaffAssignment
    {
        Task<ReadOrderStaffAssignment> CreateOrderStaffAssignmentAsync(CreateUpdateOrderStaffAssignment assignment);
        Task DeleteOrderAsync(int id);
        Task<ReadOrderStaffAssignment> GetOrderStaffAssignmentByIdAsync(int id);
        Task<IEnumerable<ReadOrderStaffAssignment>> GetOrderStaffAssignmentsAsync();
        Task<ReadOrderStaffAssignment> UpdateOrderStaffAssignmentAsync(int id, CreateUpdateOrderStaffAssignment assignment);
    }
}
