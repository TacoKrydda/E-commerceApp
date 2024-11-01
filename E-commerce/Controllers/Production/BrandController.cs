using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _service;
        public BrandController(IBrandService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostBrand")]
        public async Task<ActionResult<BrandDTO>> PostBrandAsync([FromBody] BrandDTO brand)
        {
            var entity = await _service.CreateBrandAsync(brand);
            return CreatedAtRoute("GetBrandById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteBrand")]
        public async Task<IActionResult> DeleteBrandAsync(int id)
        {
            try
            {
                await _service.DeleteBrandAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetBrandById")]
        public async Task<ActionResult<BrandDTO>> GetBrandById(int id)
        {
            var entity = await _service.GetBrandByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetBrands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrandsAsync()
        {
            var entities = await _service.GetBrandsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutBrand")]
        public async Task<IActionResult> PutBrandAsync(int id, [FromBody] BrandDTO brand)
        {
            var entity = await _service.UpdateBrandAsync(id, brand);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}