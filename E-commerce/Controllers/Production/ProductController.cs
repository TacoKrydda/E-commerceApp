using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostProduct")]
        public async Task<ActionResult<ProductDTO>> PostProductAsync(CreateUpdateProductDTO product)
        {
            var entity = await _service.CreateProductAsync(product);
            return CreatedAtRoute("GetProductById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var entity = await _service.GetProductByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            var entities = await _service.GetProductsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutProduct")]
        public async Task<IActionResult> PutProductAsync(int id, CreateUpdateProductDTO product)
        {
            var entity = await _service.UpdateProductAsync(id, product);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
