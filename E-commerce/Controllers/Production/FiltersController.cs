using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;

        public FiltersController(IBrandService brandService, ICategoryService categoryService, IColorService colorService, ISizeService sizeService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sizeService = sizeService;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            var brands = await _brandService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("colors")]
        public async Task<ActionResult<IEnumerable<string>>> GetColors()
        {
            var colors = await _colorService.GetColorsAsync();
            return Ok(colors);
        }

        [HttpGet("sizes")]
        public async Task<ActionResult<IEnumerable<string>>> GetSizes()
        {
            var sizes = await _sizeService.GetSizesAsync();
            return Ok(sizes);
        }
    }
}
