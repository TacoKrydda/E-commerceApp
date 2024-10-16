using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Models.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Production
{
    public class SizeService : ISizeService
    {
        private readonly E_commerceContext _context;
        private readonly ILogger<SizeService> _logger;
        public SizeService(E_commerceContext context, ILogger<SizeService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<SizeDTO> CreateSizeAsync(SizeDTO size)
        {
            if (size == null || string.IsNullOrWhiteSpace(size.Name))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(size.Name))
            {
                _logger.LogWarning("An Entity with the same name already exists: {EntityName}", size.Name);
                throw new InvalidOperationException("An entity with the same name already exists.");
            }

            var entity = new Size
            {
                Name = size.Name
            };

            try
            {
                await _context.Sizes.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new SizeDTO { Id = entity.Id, Name = entity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteSizeAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Sizes.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Sizes.Remove(entity);
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
            return await _context.Sizes.AnyAsync(b => b.Name == name);
        }

        public async Task<SizeDTO> GetSizeByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Sizes.FirstOrDefaultAsync(b => b.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new SizeDTO { Id = entity.Id, Name = entity.Name, };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<SizeDTO>> GetSizesAsync()
        {
            try
            {
                var entity = await _context.Sizes.ToListAsync();

                return entity.Select(b => new SizeDTO
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

        public async Task<SizeDTO> UpdateSizeAsync(int id, SizeDTO size)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Sizes.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.Name = size.Name;
                await _context.SaveChangesAsync();
                return new SizeDTO { Id = existingEntity.Id, Name = existingEntity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
