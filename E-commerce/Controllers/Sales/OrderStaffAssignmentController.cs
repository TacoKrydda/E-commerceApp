using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStaffAssignmentController : ControllerBase
    {
        private readonly IOrderStaffAssignment _service;

        public OrderStaffAssignmentController(IOrderStaffAssignment service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostOrderStaffAssignment")]
        public async Task<ActionResult<ReadOrderStaffAssignmentDTO>> PostOrderStaffAssignmentAsync([FromBody] CreateUpdateOrderStaffAssignmentDTO assignment)
        {
            var entity = await _service.CreateOrderStaffAssignmentAsync(assignment);
            return CreatedAtRoute("GetOrderStaffAssignmentById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteOrderStaffAssignment")]
        public async Task<IActionResult> DeleteOrderStaffAssignmentAsync(int id)
        {
            try
            {
                await _service.DeleteOrderStaffAssignmentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetOrderStaffAssignmentById")]
        public async Task<ActionResult<ReadOrderStaffAssignmentDTO>> GetOrderStaffAssignmentById(int id)
        {
            var entity = await _service.GetOrderStaffAssignmentByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetOrderStaffAssignments")]
        public async Task<ActionResult<IEnumerable<ReadOrderStaffAssignmentDTO>>> GetAllOrderStaffAssignmentsAsync()
        {
            var entities = await _service.GetOrderStaffAssignmentsAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutOrderStaffAssignment")]
        public async Task<IActionResult> PutOrderStaffAssignmentAsync(int id, [FromBody] CreateUpdateOrderStaffAssignmentDTO assignment)
        {
            var entity = await _service.UpdateOrderStaffAssignmentAsync(id, assignment);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}