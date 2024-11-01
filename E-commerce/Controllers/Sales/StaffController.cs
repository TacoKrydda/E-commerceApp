using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _service;

        public StaffController(IStaffService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostStaff")]
        public async Task<ActionResult<ReadStaffDTO>> PostStaffAsync([FromBody] CreateUpdateStaffDTO staff)
        {
            var entity = await _service.CreateStaffAsync(staff);
            return CreatedAtRoute("GetStaffById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteStaff")]
        public async Task<IActionResult> DeleteStaffAsync(int id)
        {
            try
            {
                await _service.DeleteStaffAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetStaffById")]
        public async Task<ActionResult<ReadStaffDTO>> GetStaffById(int id)
        {
            var entity = await _service.GetStaffByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetStaffs")]
        public async Task<ActionResult<IEnumerable<ReadStaffDTO>>> GetAllStaffsAsync()
        {
            var entities = await _service.GetStaffsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutStaff")]
        public async Task<IActionResult> PutStaffAsync(int id, [FromBody] CreateUpdateStaffDTO staff)
        {
            var entity = await _service.UpdateStaffAsync(id, staff);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
