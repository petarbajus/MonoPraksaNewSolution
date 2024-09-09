using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewSolution.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FootballerController : ControllerBase
    {
        [HttpPost("insertFootballer")]
        public async Task<IActionResult> InsertFootballerAsync([FromBody] Footballer footballer)
        {
            var footballerService = new FootballerService();
            if (footballer == null)
            {
                return BadRequest("Footballer object is null.");
            }

            var success = await footballerService.InsertFootballerAsync(footballer);
            return success ? Ok() : BadRequest("Failed to insert footballer.");
        }

        [HttpDelete("deleteFootballerById/{id}")]
        public async Task<IActionResult> DeleteFootballerByIdAsync(Guid id)
        {
            var footballerService = new FootballerService();
            var success = await footballerService.DeleteFootballerByIdAsync(id);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpPut("updateFootballerById/{id}")]
        public async Task<IActionResult> UpdateFootballerByIdAsync(Guid id, [FromBody] Footballer footballer)
        {
            var footballerService = new FootballerService();
            var success = await footballerService.UpdateFootballerByIdAsync(id, footballer);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballerById/{id}")]
        public async Task<IActionResult> GetFootballerByIdAsync(Guid id)
        {
            var footballerService = new FootballerService();
            var footballer = await footballerService.GetFootballerByIdAsync(id);
            return footballer != null ? Ok(footballer) : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballers")]
        public async Task<IActionResult> GetFootballersAsync()
        {
            var footballerService = new FootballerService();
            var footballers = await footballerService.GetFootballersAsync();
            return footballers.Any() ? Ok(footballers) : NotFound("No footballers found.");
        }
    }
}
