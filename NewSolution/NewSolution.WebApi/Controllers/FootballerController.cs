using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service;
using NewSolution.Service.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NewSolution.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FootballerController : ControllerBase
    {
        private IFootballerService _footballerService;

        public FootballerController(IFootballerService footballerService)
        {
            _footballerService = footballerService;
        }

        [HttpPost("insertFootballer")]
        public async Task<IActionResult> InsertFootballerAsync([FromBody] Footballer footballer)
        {
            if (footballer == null)
            {
                return BadRequest("Footballer object is null.");
            }

            var success = await _footballerService.InsertFootballerAsync(footballer);
            return success ? Ok() : BadRequest("Failed to insert footballer.");
        }

        [HttpDelete("deleteFootballerById/{id}")]
        public async Task<IActionResult> DeleteFootballerByIdAsync([Required] Guid id)
        {
            var success = await _footballerService.DeleteFootballerByIdAsync(id);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpPut("updateFootballerById/{id}")]
        public async Task<IActionResult> UpdateFootballerByIdAsync([Required] Guid id, [FromBody] Footballer footballer)
        {
            var success = await _footballerService.UpdateFootballerByIdAsync(id, footballer);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballerById/{id}")]
        public async Task<IActionResult> GetFootballerByIdAsync([Required] Guid id)
        {
            var footballer = await _footballerService.GetFootballerByIdAsync(id);
            return footballer != null ? Ok(footballer) : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballers")]
        public async Task<IActionResult> GetFootballersAsync()
        {
            var footballers = await _footballerService.GetFootballersAsync();
            return footballers.Any() ? Ok(footballers) : NotFound("No footballers found.");
        }
    }
}
