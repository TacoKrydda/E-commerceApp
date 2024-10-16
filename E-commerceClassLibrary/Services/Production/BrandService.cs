using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Models.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Production
{
    public class BrandService : IBrandService
    {
        private readonly E_commerceContext _context;
        private readonly ILogger<BrandService> _logger;
        public BrandService(E_commerceContext context, ILogger<BrandService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<BrandDTO> CreateBrandAsync(BrandDTO brand)
        {
            if (brand == null || string.IsNullOrWhiteSpace(brand.Name))
            {
                _logger.LogWarning("Invalid brand data provided.");
                throw new ArgumentException("Brand data is invalid.");
            }

            if (await EntityExistsAsync(brand.Name))
            {
                _logger.LogWarning("An Entity with the same name already exists: {EntityName}", brand.Name);
                throw new InvalidOperationException("An entity with the same name already exists.");
            }

            var entity = new Brand
            {
                Name = brand.Name
            };

            try
            {
                await _context.Brands.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new BrandDTO { Id = entity.Id, Name = entity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteBrandAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Brands.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Brands.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(string name)
        {
            return await _context.Brands.AnyAsync(b => b.Name == name);
        }

        public async Task<BrandDTO> GetBrandByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new BrandDTO { Id = entity.Id, Name = entity.Name, };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<BrandDTO>> GetBrandsAsync()
        {
            try
            {
                var entity = await _context.Brands.ToListAsync();

                return entity.Select(b => new BrandDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<BrandDTO> UpdateBrandAsync(int id, BrandDTO brand)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Brands.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.Name = brand.Name;
                await _context.SaveChangesAsync();
                return new BrandDTO { Id = existingEntity.Id, Name = existingEntity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
