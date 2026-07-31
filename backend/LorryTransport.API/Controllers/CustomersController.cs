using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IGenericRepository<Customer> _repository;

        public CustomersController(IGenericRepository<Customer> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            await _repository.AddAsync(customer);
            await _repository.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            _repository.Delete(existing);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
