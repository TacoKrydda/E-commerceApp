using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;

        public CartItemController(ICartItemService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostCartItem")]
        public async Task<ActionResult<ReadCartItemDTO>> PostCartItemAsync([FromBody] CreateUpdateCartItemDTO cartItem)
        {
            var entity = await _service.CreateCartItemAsync(cartItem);
            return CreatedAtRoute("GetCartItemById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteCartItem")]
        public async Task<IActionResult> DeleteCartItemAsync(int id)
        {
            try
            {
                await _service.DeleteCartItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetCartItemById")]
        public async Task<ActionResult<ReadCartItemDTO>> GetCartItemById(int id)
        {
            var entity = await _service.GetCartItemByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetCartItems")]
        public async Task<ActionResult<IEnumerable<ReadCartItemDTO>>> GetAllCartItemsAsync()
        {
            var entities = await _service.GetCartItemsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutCartItem")]
        public async Task<IActionResult> PutCartItemAsync(int id, [FromBody] CreateUpdateCartItemDTO cartItem)
        {
            var entity = await _service.UpdateCartItemAsync(id, cartItem);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
