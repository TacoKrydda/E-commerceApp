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
    public class StaffService : IStaffService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<StaffService> _logger;
        private readonly IMapper _mapper;

        public StaffService(EcommerceContext context, ILogger<StaffService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadStaffDTO> CreateStaffAsync(CreateUpdateStaffDTO staff)
        {
            if (staff == null || string.IsNullOrWhiteSpace(staff.Email))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(staff.Email))
            {
                _logger.LogWarning("An Entity with the same mail already exists: {EntityMail}", staff.Email);
                throw new InvalidOperationException("An entity with the same mail already exists.");
            }

            var entity = new Staff
            {
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Email = staff.Email,
                Phone = staff.Phone
            };

            try
            {
                await _context.Staff.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ReadStaffDTO
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteStaffAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Staff.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Staff.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(string email)
        {
            return await _context.Staff.AnyAsync(s => s.Email == email);
        }

        public async Task<ReadStaffDTO> GetStaffByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Staff.FirstOrDefaultAsync(s => s.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new ReadStaffDTO
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ReadStaffDTO>> GetStaffsAsync()
        {
            try
            {
                var entity = await _context.Staff.ToListAsync();

                return entity.Select(s => new ReadStaffDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Phone
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ReadStaffDTO> UpdateStaffAsync(int id, CreateUpdateStaffDTO staff)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Staff.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _mapper.Map(staff, existingEntity);

                await _context.SaveChangesAsync();
                return _mapper.Map<ReadStaffDTO>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
