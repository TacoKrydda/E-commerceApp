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

            if (await EntityExistsAsync(product.Name, product.BrandId, product.ColorId, product.SizeId))
            {
                _logger.LogWarning("A product with the same name, brand, color, size already exists: {ProductName}, {BrandId}, {ColorId} ,{SizeId}", product.Name, product.BrandId, product.ColorId, product.SizeId);
                throw new InvalidOperationException("A product with the same name, brand, color, size already exists.");
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
                ImagePath = product.ImagePath,
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

                var entityFromDb = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Stock)
                    .FirstOrDefaultAsync(p => p.Id == entity.Id);

                return new ProductDTO
                {
                    Id = entityFromDb?.Id ?? 0,
                    Name = entityFromDb?.Name ?? string.Empty,
                    Brand = entityFromDb?.Brand?.Name ?? string.Empty,
                    Category = entityFromDb?.Category?.Name ?? string.Empty,
                    Color = entityFromDb?.Color?.Name ?? string.Empty,
                    Size = entityFromDb?.Size?.Name ?? string.Empty,
                    Price = entityFromDb?.Price ?? 0,
                    Stock = entityFromDb?.Stock?.Quantity ?? 0,
                    ImagePath = entityFromDb?.ImagePath ?? string.Empty,
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

        public async Task<bool> EntityExistsAsync(string name, int brandId, int colorId, int sizeId)
        {
            return await _context.Products.AnyAsync(p => p.Name == name && p.BrandId == brandId && p.ColorId == colorId && p.SizeId == sizeId);
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
                    ImagePath = entity.ImagePath,
                    Stock = entity?.Stock?.Quantity ?? 0,
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync(
            string? color,
            string? size,
            string? category,
            string? brand)
        {
            // Basquery för att inkludera nödvändiga relationer
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .Include(p => p.Stock)
                .AsQueryable();

            // Lägg till dynamiska filter
            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.Color.Name == color);
            }

            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.Size.Name == size);
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name == category);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand.Name == brand);
            }

            // Gruppera produkterna och välj första variationen
            var groupedProducts = await query
                .GroupBy(p => new { p.Name, p.BrandId, p.CategoryId })
                .Select(g => g.First())
                .ToListAsync();

            // Mappa till DTO
            return groupedProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand?.Name ?? string.Empty,
                Category = p.Category?.Name ?? string.Empty,
                Color = p.Color?.Name ?? string.Empty,
                Size = p.Size?.Name ?? string.Empty,
                Price = p.Price,
                ImagePath = p.ImagePath,
                Stock = p.Stock?.Quantity ?? 0
            }).ToList();
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
                existingEntity.ImagePath = product.ImagePath;
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
                    ImagePath = existingEntity.ImagePath,
                    Stock = existingEntity.Stock.Quantity
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductByNameAsync(string name)
        {
            if (name == null || string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            try
            {
                var entity = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Stock)
                    .Where(p => p.Name == name)
                    .ToListAsync();

                if (entity == null)
                {
                    _logger.LogWarning("Entity with Name {Name} not found.", name);
                    throw new KeyNotFoundException($"Entity with Name {name} not found.");
                }
                return entity.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand?.Name ?? string.Empty,
                    Category = p.Category?.Name ?? string.Empty,
                    Color = p.Color?.Name ?? string.Empty,
                    Size = p.Size?.Name ?? string.Empty,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    Stock = p?.Stock?.Quantity ?? 0,
                });
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }
    }
}
