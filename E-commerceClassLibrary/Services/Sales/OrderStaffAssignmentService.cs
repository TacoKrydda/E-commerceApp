using AutoMapper;
using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.Extensions.Logging;

namespace E_commerceClassLibrary.Services.Sales
{
    public class OrderStaffAssignmentService : IOrderStaffAssignment
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<OrderStaffAssignmentService> _logger;
        private readonly IMapper _mapper;

        public OrderStaffAssignmentService(EcommerceContext context, ILogger<OrderStaffAssignmentService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ReadOrderStaffAssignment> CreateOrderStaffAssignmentAsync(CreateUpdateOrderStaffAssignment assignment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReadOrderStaffAssignment> GetOrderStaffAssignmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReadOrderStaffAssignment>> GetOrderStaffAssignmentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ReadOrderStaffAssignment> UpdateOrderStaffAssignmentAsync(int id, CreateUpdateOrderStaffAssignment assignment)
        {
            throw new NotImplementedException();
        }
    }
}
