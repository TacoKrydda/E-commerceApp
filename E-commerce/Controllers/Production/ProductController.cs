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
    }
}
