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
    public class CartItemService : ICartItemService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<CartItemService> _logger;
        private readonly IMapper _mapper;

        public CartItemService(EcommerceContext context, ILogger<CartItemService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadCartItemDTO> CreateCartItemAsync(CreateUpdateCartItemDTO cartItem)
        {
            if (cartItem == null)
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            if (await EntityExistsAsync(cartItem.OrderId, cartItem.ProductId))
            {
                _logger.LogWarning("An Entity with the same orderId & productId already exists: {OrderId} {ProductId}", cartItem.OrderId, cartItem.ProductId);
                throw new InvalidOperationException("An entity with the same id already exists.");
            }

            var entity = _mapper.Map<CartItem>(cartItem);

            try
            {
                await _context.CartItems.AddAsync(entity);
                await _context.SaveChangesAsync();
                var entityFromDb = await _context.CartItems
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci => ci.Id == entity.Id);
                return _mapper.Map<ReadCartItemDTO>(entityFromDb);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteCartItemAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.CartItems.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.CartItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<bool> EntityExistsAsync(int OrderId, int productId)
        {
            return await _context.CartItems.AnyAsync(ci => ci.OrderId == OrderId && ci.ProductId == productId);
        }

        public async Task<ReadCartItemDTO> GetCartItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.CartItems
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci => ci.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return _mapper.Map<ReadCartItemDTO>(entity);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ReadCartItemDTO>> GetCartItemsAsync()
        {
            try
            {
                var entity = await _context.CartItems
                    .Include(ci => ci.Product)
                    .ToListAsync();

                return entity.Select(ci => _mapper.Map<ReadCartItemDTO>(ci)).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ReadCartItemDTO> UpdateCartItemAsync(int id, CreateUpdateCartItemDTO cartItem)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.CartItems
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci => ci.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _mapper.Map(cartItem, existingEntity);

                await _context.SaveChangesAsync();
                return _mapper.Map<ReadCartItemDTO>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
