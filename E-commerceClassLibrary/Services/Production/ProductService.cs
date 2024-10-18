using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Models.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Production
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<ProductService> _logger;
        public ProductService(EcommerceContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ProductDTO> CreateProductAsync(CreateUpdateProductDTO product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(product.Name, product.BrandId))
            {
                _logger.LogWarning("A product with the same name and brand already exists: {ProductName}, {BrandId}", product.Name, product.BrandId);
                throw new InvalidOperationException("A product with the same name and brand already exists.");
            }

            //using var transaction = await _context.Database.BeginTransactionAsync(); // Startar transaktionen

            var entity = new Product
            {
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ColorId = product.ColorId,
                SizeId = product.SizeId,
                Price = product.Price,
            };

            try
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();

                var stock = new Stock
                {
                    ProductId = entity.Id,
                    Quantity = product.StockQuantity,
                };
                await _context.Stocks.AddAsync(stock);
                await _context.SaveChangesAsync();

                //await transaction.CommitAsync(); // Bekräfta transaktionen

                return new ProductDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Brand = entity?.Brand?.Name ?? string.Empty,
                    Category = entity?.Category?.Name ?? string.Empty,
                    Color = entity?.Color?.Name ?? string.Empty,
                    Size = entity?.Size?.Name ?? string.Empty,
                    Price = entity.Price,
                    Stock = entity?.Stock?.Quantity ?? 0,
                };
            }
            catch (DbUpdateException ex)
            {
                //await transaction.RollbackAsync(); // Rulla tillbaka om något går fel
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Products.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(string name, int brandId)
        {
            return await _context.Products.AnyAsync(p => p.Name == name && p.BrandId == brandId);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Stock)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return new ProductDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Brand = entity.Brand?.Name ?? string.Empty,
                    Category = entity.Category?.Name ?? string.Empty,
                    Color = entity.Color?.Name ?? string.Empty,
                    Size = entity.Size?.Name ?? string.Empty,
                    Price = entity.Price,
                    Stock = entity?.Stock?.Quantity ?? 0,
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            try
            {
                var entity = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Stock)
                    .ToListAsync();

                return entity.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand?.Name ?? string.Empty,
                    Category = p.Category?.Name ?? string.Empty,
                    Color = p.Color?.Name ?? string.Empty,
                    Size = p.Size?.Name ?? string.Empty,
                    Price = p.Price,
                    Stock = p.Stock?.Quantity ?? 0
                }).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, CreateUpdateProductDTO product)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Stock)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.Name = product.Name;
                existingEntity.BrandId = product.BrandId;
                existingEntity.CategoryId = product.CategoryId;
                existingEntity.ColorId = product.ColorId;
                existingEntity.SizeId = product.SizeId;
                existingEntity.Price = product.Price;
                if (existingEntity.Stock != null)
                {
                    if (product.StockQuantity < 0)
                    {
                        _logger.LogWarning("Invalid stock quantity for product ID {Id}", id);
                        throw new ArgumentException("Stock quantity cannot be negative.");
                    }

                    existingEntity.Stock.Quantity = product.StockQuantity;
                }
                else
                {
                    // Skapa en ny lagerpost om ingen finns
                    existingEntity.Stock = new Stock
                    {
                        ProductId = existingEntity.Id,
                        Quantity = product.StockQuantity
                    };
                }

                await _context.SaveChangesAsync();
                return new ProductDTO
                {
                    Id = existingEntity.Id,
                    Name = existingEntity.Name,
                    Brand = existingEntity.Brand?.Name ?? string.Empty,
                    Category = existingEntity.Category?.Name ?? string.Empty,
                    Color = existingEntity.Color?.Name ?? string.Empty,
                    Size = existingEntity.Size?.Name ?? string.Empty,
                    Price = existingEntity.Price,
                    Stock = existingEntity.Stock.Quantity
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
