using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostCategory")]
        public async Task<ActionResult<CategoryDTO>> PostCategoryAsync(CategoryDTO category)
        {
            var entity = await _service.CreateCategoryAsync(category);
            return CreatedAtRoute("GetCategoryById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                await _service.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var entity = await _service.GetCategoryByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            var entities = await _service.GetCategoriesAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutCategory")]
        public async Task<IActionResult> PutCategoryAsync(int id, CategoryDTO category)
        {
            var entity = await _service.UpdateCategoryAsync(id, category);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
