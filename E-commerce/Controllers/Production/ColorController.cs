using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _service;
        public ColorController(IColorService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostColor")]
        public async Task<ActionResult<ColorDTO>> PostColorAsync([FromBody] ColorDTO color)
        {
            var entity = await _service.CreateColorAsync(color);
            return CreatedAtRoute("GetColorById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteColor")]
        public async Task<IActionResult> DeleteColorAsync(int id)
        {
            try
            {
                await _service.DeleteColorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetColorById")]
        public async Task<ActionResult<ColorDTO>> GetColorById(int id)
        {
            var entity = await _service.GetColorByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetColors")]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetAllColorsAsync()
        {
            var entities = await _service.GetColorsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutColor")]
        public async Task<IActionResult> PutColorAsync(int id, [FromBody] ColorDTO color)
        {
            var entity = await _service.UpdateColorAsync(id, color);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
