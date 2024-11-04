using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostOrder")]
        public async Task<ActionResult<ReadOrderDTO>> PostOrderAsync([FromBody] CreateUpdateOrderDTO order)
        {
            var entity = await _service.CreateOrderAsync(order);
            return CreatedAtRoute("GetOrderById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                await _service.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<ActionResult<ReadOrderDTO>> GetOrderById(int id)
        {
            var entity = await _service.GetOrderByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<ActionResult<IEnumerable<ReadOrderDTO>>> GetAllOrdersAsync()
        {
            var entities = await _service.GetOrdersAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutOrder")]
        public async Task<IActionResult> PutOrderAsync(int id, [FromBody] CreateUpdateOrderDTO order)
        {
            var entity = await _service.UpdateOrderAsync(id, order);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
