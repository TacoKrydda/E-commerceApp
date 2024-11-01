using E_commerceClassLibrary.DTO.Sales;
using E_commerceClassLibrary.Interfaces.Sales;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpPost(Name = "PostCustomer")]
        public async Task<ActionResult<ReadCustomerDTO>> PostCustomerAsync([FromBody] CreateUpdateCustomerDTO customer)
        {
            var entity = await _service.CreateCustomerAsync(customer);
            return CreatedAtRoute("GetCustomerById", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                await _service.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<ActionResult<ReadCustomerDTO>> GetCustomerById(int id)
        {
            var entity = await _service.GetCustomerByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<ActionResult<IEnumerable<ReadCustomerDTO>>> GetAllCustomersAsync()
        {
            var entities = await _service.GetCustomersAsync();
            return Ok(entities);
        }

        [HttpPut("{id}", Name = "PutCustomer")]
        public async Task<IActionResult> PutCustomerAsync(int id, [FromBody] CreateUpdateCustomerDTO customer)
        {
            var entity = await _service.UpdateCustomerAsync(id, customer);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
