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
    public class CustomerService : ICustomerService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _mapper;
        public CustomerService(EcommerceContext context, ILogger<CustomerService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadCustomerDTO> CreateCustomerAsync(CreateUpdateCustomerDTO customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.Email))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(customer.Email))
            {
                _logger.LogWarning("An Entity with the same mail already exists: {EntityMail}", customer.Email);
                throw new InvalidOperationException("An entity with the same mail already exists.");
            }

            var entity = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Street = customer.Street,
                City = customer.City,
            };

            try
            {
                await _context.Customers.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ReadCustomerDTO
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    Street = entity.Street,
                    City = entity.City,
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Customers.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Customers.Remove(entity);
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
            return await _context.Customers.AnyAsync(c => c.Email == email);
        }

        public async Task<ReadCustomerDTO> GetCustomerByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new ReadCustomerDTO
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    Street = entity.Street,
                    City = entity.City,
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ReadCustomerDTO>> GetCustomersAsync()
        {
            try
            {
                var entity = await _context.Customers.ToListAsync();

                return entity.Select(c => new ReadCustomerDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Street = c.Street,
                    City = c.City,
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ReadCustomerDTO> UpdateCustomerAsync(int id, CreateUpdateCustomerDTO customer)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Customers.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _mapper.Map(customer, existingEntity);

                await _context.SaveChangesAsync();
                return _mapper.Map<ReadCustomerDTO>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
