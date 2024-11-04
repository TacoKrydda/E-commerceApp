using AutoMapper;
using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using E_commerceClassLibrary.Models.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

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

        public async Task<ReadOrderStaffAssignmentDTO> CreateOrderStaffAssignmentAsync(CreateUpdateOrderStaffAssignmentDTO assignment)
        {
            if (assignment == null)
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }
            if (await EntityExistsAsync(assignment.StaffId))
            {
                _logger.LogWarning("An Entity with the same staffId already exists: {staffId}", assignment.StaffId);
                throw new InvalidOperationException("An entity with the same staffId already exists.");
            }

            var entity = new OrderStaffAssignment
            {
                OrderId = assignment.OrderId,
                StaffId = assignment.StaffId,
            };

            try
            {
                await _context.OrderStaffAssignments.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new ReadOrderStaffAssignmentDTO
                {
                    Id = entity.Id,
                    OrderId = assignment.OrderId,
                    StaffId = assignment.StaffId,
                    StaffName = (await _context.Staff.FindAsync(entity.StaffId))?.FirstName ?? string.Empty,
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteOrderStaffAssignmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.OrderStaffAssignments.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.OrderStaffAssignments.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(int staffId)
        {
            return await _context.Staff.AnyAsync(s => s.Id == staffId);
        }

        public async Task<ReadOrderStaffAssignmentDTO> GetOrderStaffAssignmentByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.OrderStaffAssignments.FirstOrDefaultAsync(s => s.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new ReadOrderStaffAssignmentDTO
                {
                    Id = entity.Id,
                    OrderId = entity.OrderId,
                    StaffId = entity.StaffId,
                    StaffName = entity.Staff.FirstName
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ReadOrderStaffAssignmentDTO>> GetOrderStaffAssignmentsAsync()
        {
            try
            {
                var entity = await _context.OrderStaffAssignments
                    .Include(s => s.Staff)
                    .ToListAsync();

                return entity.Select(oSA => new ReadOrderStaffAssignmentDTO
                {
                    Id = oSA.Id,
                    OrderId = oSA.OrderId,
                    StaffId = oSA.StaffId,
                    StaffName = oSA.Staff.FirstName
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ReadOrderStaffAssignmentDTO> UpdateOrderStaffAssignmentAsync(int id, CreateUpdateOrderStaffAssignmentDTO assignment)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.OrderStaffAssignments.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _mapper.Map(assignment, existingEntity);

                await _context.SaveChangesAsync();
                return _mapper.Map<ReadOrderStaffAssignmentDTO>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
