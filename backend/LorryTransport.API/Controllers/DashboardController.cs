using LorryTransport.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LorryTransport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILoadEntryService _service;

        public DashboardController(ILoadEntryService service)
        {
            _service = service;
        }

        // GET api/dashboard
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var dashboard = await _service.GetDashboardAsync();
            return Ok(dashboard);
        }
    }
}
