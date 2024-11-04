using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Models.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Production
{
    public class ColorService : IColorService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<ColorService> _logger;
        public ColorService(EcommerceContext context, ILogger<ColorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ColorDTO> CreateColorAsync(ColorDTO color)
        {
            if (color == null || string.IsNullOrWhiteSpace(color.Name))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(color.Name))
            {
                _logger.LogWarning("An Entity with the same name already exists: {EntityName}", color.Name);
                throw new InvalidOperationException("An entity with the same name already exists.");
            }

            var entity = new Color
            {
                Name = color.Name
            };

            try
            {
                await _context.Colors.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ColorDTO { Id = entity.Id, Name = entity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteColorAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Colors.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Colors.Remove(entity);
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
            return await _context.Colors.AnyAsync(c => c.Name == name);
        }

        public async Task<ColorDTO> GetColorByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new ColorDTO { Id = entity.Id, Name = entity.Name, };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ColorDTO>> GetColorsAsync()
        {
            try
            {
                var entity = await _context.Categories.ToListAsync();

                return entity.Select(c => new ColorDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ColorDTO> UpdateColorAsync(int id, ColorDTO color)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Colors.FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.Name = color.Name;
                await _context.SaveChangesAsync();
                return new ColorDTO { Id = existingEntity.Id, Name = existingEntity.Name };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
