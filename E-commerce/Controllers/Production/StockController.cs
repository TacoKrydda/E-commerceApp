using E_commerceClassLibrary.DTO.Production;
using E_commerceClassLibrary.Interfaces.Production;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Production
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;
        public StockController(IStockService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostStock")]
        public async Task<ActionResult<StockDTO>> PostStockAsync([FromBody] CreateUpdateStockDTO Stock)
        {
            var entity = await _service.CreateStockAsync(Stock);
            return CreatedAtRoute("GetStockById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteStock")]
        public async Task<IActionResult> DeleteStockAsync(int id)
        {
            try
            {
                await _service.DeleteStockAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetStockById")]
        public async Task<ActionResult<StockDTO>> GetStockById(int id)
        {
            var entity = await _service.GetStockByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetStockts")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetAllStocksAsync()
        {
            var entities = await _service.GetStocksAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutStock")]
        public async Task<IActionResult> PutStockAsync(int id, [FromBody] CreateUpdateStockDTO stock)
        {
            var entity = await _service.UpdateStockAsync(id, stock);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
