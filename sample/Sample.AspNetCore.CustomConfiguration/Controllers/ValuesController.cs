using Microsoft.AspNetCore.Mvc;

using System;

namespace Sample.AspNetCore.CustomConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost("process")]
        public IActionResult Process()
        {
            return Ok(new Random().NextDouble());
        }
    }
}
