using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Models.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Production
{
    public class StockService : IStockService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<StockService> _logger;
        public StockService(EcommerceContext context, ILogger<StockService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StockDTO> CreateStockAsync(CreateUpdateStockDTO stock)
        {
            if (stock == null)
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (stock.Quantity < 0)
            {
                _logger.LogWarning("Invalid stock quantity {Quantity}", stock.Quantity);
                throw new ArgumentException("Stock quantity cannot be negative.");
            }

            if (await EntityExistsAsync(stock.ProductId))
            {
                _logger.LogWarning("A stock with the same productId already exists: {productId}", stock.ProductId);
                throw new InvalidOperationException("A stock with the same productId already exists.");
            }

            var entity = new Stock
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity,
            };

            try
            {
                await _context.Stocks.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new StockDTO
                {
                    Id = entity.Id,
                    Product = (await _context.Stocks.FindAsync(entity.ProductId))?.Product?.Name ?? string.Empty,
                    Quantity = entity.Quantity,
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteStockAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Stocks.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Stocks.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(int productId)
        {
            return await _context.Stocks.AnyAsync(s => s.ProductId == productId);
        }

        public async Task<StockDTO> GetStockByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Stocks
                    .Include(s => s.Product)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new StockDTO
                {
                    Id = entity.Id,
                    Product = entity.Product?.Name ?? string.Empty,
                    Quantity = entity.Quantity,

                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<StockDTO>> GetStocksAsync()
        {
            try
            {
                var entity = await _context.Stocks
                    .Include(s => s.Product)
                    .ToListAsync();

                return entity.Select(s => new StockDTO
                {
                    Id = s.Id,
                    Product = s.Product?.Name ?? string.Empty,
                    Quantity = s.Quantity,
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<StockDTO> UpdateStockAsync(int id, CreateUpdateStockDTO stock)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            if (stock.Quantity < 0)
            {
                _logger.LogWarning("Invalid stock quantity for stock ID {Id}", id);
                throw new ArgumentException("Stock quantity cannot be negative.");
            }

            try
            {
                var existingEntity = await _context.Stocks
                    .Include(s => s.Product)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.ProductId = stock.ProductId;
                existingEntity.Quantity = stock.Quantity;


                await _context.SaveChangesAsync();
                return new StockDTO
                {
                    Id = existingEntity.Id,
                    Product = existingEntity.Product?.Name ?? string.Empty,
                    Quantity = existingEntity.Quantity,
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
