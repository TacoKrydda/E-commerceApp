using AutoMapper;
using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Interfaces.Sales;
using E_commerceClassLibrary.Models.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace E_commerceClassLibrary.Services.Sales
{
    public class OrderService : IOrderService
    {
        private readonly EcommerceContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        public OrderService(EcommerceContext context, ILogger<OrderService> logger, IMapper mapper, IStockService stockService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _stockService = stockService;
        }

        public async Task<ReadOrderDTO> CreateOrderAsync(CreateUpdateOrderDTO order)
        {
            if (order == null)
            {
                _logger.LogWarning("Invalid entity data provided.");
                throw new ArgumentException("Entity data is invalid.");
            }

            var entity = _mapper.Map<Order>(order);

            try
            {
                await _context.Orders.AddAsync(entity);
                await _context.SaveChangesAsync();

                var cartItems = await _context.CartItems
                    .Where(ci => ci.OrderId == entity.Id)
                    .Include(ci => ci.Product)
                    .Select(ci => _mapper.Map<ReadCartItemDTO>(ci))
                    .ToListAsync();

                decimal totalPrice = cartItems.Sum(ci => ci.Quantity * ci.Price);
                entity.TotalPrice = totalPrice;

                foreach (var cartItem in order.CartItems)
                {
                    await _stockService.AdjustStockAsync(cartItem.ProductId, -cartItem.Quantity);
                }

                _context.Update(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<ReadOrderDTO>(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Orders.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while removing the entity.");
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public async Task<ReadOrderDTO> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var entity = await _context.Orders
                    .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                return _mapper.Map<ReadOrderDTO>(entity);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entity.");
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<ReadOrderDTO>> GetOrdersAsync()
        {
            try
            {
                var entity = await _context.Orders
                    .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .ToListAsync();

                return entity.Select(o => _mapper.Map<ReadOrderDTO>(o)).ToList();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the entities.");
                throw new InvalidOperationException("An error occurred while retrieving the entities.", ex);
            }
        }

        public async Task<ReadOrderDTO> UpdateOrderAsync(int id, CreateUpdateOrderDTO order)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID value", nameof(id));
            }

            try
            {
                var existingEntity = await _context.Orders
                    .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }

                existingEntity.CustomerId = order.CustomerId;
                existingEntity.OrderStatus = order.OrderStatus;
                existingEntity.OrderDate = order.OrderDate;
                existingEntity.ShippedDate = order.ShippedDate;

                var cartItemsToRemove = existingEntity.CartItems
                    .Where(ci => !order.CartItems.Any(oci => oci.ProductId == ci.ProductId && oci.OrderId == ci.OrderId))
                    .ToList();

                foreach (var cartItem in cartItemsToRemove)
                {
                    _context.CartItems.Remove(cartItem);
                }

                foreach (var cartItem in order.CartItems)
                {
                    var cartItemToUpdate = existingEntity.CartItems
                        .FirstOrDefault(ci => ci.ProductId == cartItem.ProductId && ci.OrderId == cartItem.OrderId);

                    if (cartItemToUpdate != null)
                    {
                        int quantityDifference = cartItem.Quantity - cartItemToUpdate.Quantity;
                        await _stockService.AdjustStockAsync(cartItem.ProductId, -quantityDifference);

                        cartItemToUpdate.Quantity = cartItem.Quantity;
                    }
                    else
                    {
                        existingEntity.CartItems.Add(new CartItem
                        {
                            ProductId = cartItem.ProductId,
                            OrderId = cartItem.OrderId,
                            Quantity = cartItem.Quantity,
                        });

                        await _stockService.AdjustStockAsync(cartItem.ProductId, -cartItem.Quantity);
                    }
                }
                await _context.SaveChangesAsync();

                var updatedEntityFromDB = await _context.Orders
                    .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                decimal totalPrice = updatedEntityFromDB.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
                updatedEntityFromDB.TotalPrice = totalPrice;

                await _context.SaveChangesAsync();

                return _mapper.Map<ReadOrderDTO>(updatedEntityFromDB);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity.");
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }

    }
}
