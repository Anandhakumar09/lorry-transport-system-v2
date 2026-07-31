using LorryTransport.Application.Interfaces;
using LorryTransport.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly IGenericRepository<Driver> _repository;

        public DriversController(IGenericRepository<Driver> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var driver = await _repository.GetByIdAsync(id);
            if (driver == null) return NotFound();
            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Driver driver)
        {
            await _repository.AddAsync(driver);
            await _repository.SaveChangesAsync();
            return Ok(driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Driver driver)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = driver.Name;
            existing.PhoneNumber = driver.PhoneNumber;
            existing.LicenseNumber = driver.LicenseNumber;

            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return NoContent();
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
