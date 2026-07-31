using LorryTransport.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverLedgerController : ControllerBase
    {
        private readonly IDriverLedgerService _service;

        public DriverLedgerController(IDriverLedgerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllLedgersAsync());

        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetByDriver(int driverId)
        {
            var ledger = await _service.GetLedgerByDriverIdAsync(driverId);
            if (ledger == null) return NotFound();
            return Ok(ledger);
        }
    }
}
