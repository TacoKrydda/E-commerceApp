using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _service;
        public SizeController(ISizeService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostSize")]
        public async Task<ActionResult<SizeDTO>> PostSizeAsync(SizeDTO size)
        {
            var entity = await _service.CreateSizeAsync(size);
            return CreatedAtRoute("GetSizeById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteSize")]
        public async Task<IActionResult> DeleteSizeAsync(int id)
        {
            try
            {
                await _service.DeleteSizeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetSizeById")]
        public async Task<ActionResult<SizeDTO>> GetSizeById(int id)
        {
            var entity = await _service.GetSizeByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetSizes")]
        public async Task<ActionResult<IEnumerable<SizeDTO>>> GetAllSizesAsync()
        {
            var entities = await _service.GetSizesAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutSize")]
        public async Task<IActionResult> PutSizeAsync(int id, SizeDTO size)
        {
            var entity = await _service.UpdateSizeAsync(id, size);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
