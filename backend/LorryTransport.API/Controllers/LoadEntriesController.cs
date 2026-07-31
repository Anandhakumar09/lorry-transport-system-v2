using LorryTransport.Application.DTOs;
using LorryTransport.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoadEntriesController : ControllerBase
    {
        private readonly ILoadEntryService _service;

        public LoadEntriesController(ILoadEntryService service)
        {
            _service = service;
        }

        // GET api/loadentries
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // GET api/loadentries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _service.GetByIdAsync(id);
            if (entry == null) return NotFound(new { message = "Load entry not found" });
            return Ok(entry);
        }

        // POST api/loadentries
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoadEntryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/loadentries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateLoadEntryDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound(new { message = "Load entry not found" });
            return NoContent();
        }

        // DELETE api/loadentries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound(new { message = "Load entry not found" });
            return NoContent();
        }
    }
}
