using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IGenericRepository<Vehicle> _repository;

        public VehiclesController(IGenericRepository<Vehicle> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vehicle vehicle)
        {
            await _repository.AddAsync(vehicle);
            await _repository.SaveChangesAsync();
            return Ok(vehicle);
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
