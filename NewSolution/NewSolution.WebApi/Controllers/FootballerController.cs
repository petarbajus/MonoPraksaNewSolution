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
        public IActionResult InsertFootballer([FromBody] Footballer footballer)
        {
            var footballerService = new FootballerService();
            if (footballer == null)
            {
                return BadRequest("Footballer object is null.");
            }

            var success = footballerService.InsertFootballer(footballer);
            return success ? Ok() : BadRequest("Failed to insert footballer.");
        }

        [HttpDelete("deleteFootballerById/{id}")]
        public IActionResult DeleteFootballerById(Guid id)
        {
            var footballerService = new FootballerService();
            var success = footballerService.DeleteFootballerById(id);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpPut("updateFootballerById/{id}")]
        public IActionResult UpdateFootballerById(Guid id, [FromBody] Footballer footballer)
        {
            var footballerService = new FootballerService();
            var success = footballerService.UpdateFootballerById(id, footballer);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballerById/{id}")]
        public IActionResult GetFootballerById(Guid id)
        {
            var footballerService = new FootballerService();
            var footballer = footballerService.GetFootballerById(id);
            return footballer != null ? Ok(footballer) : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballers")]
        public IActionResult GetFootballers()
        {
            var footballerService = new FootballerService();
            var footballers = footballerService.GetFootballers();
            return footballers.Any() ? Ok(footballers) : NotFound("No footballers found.");
        }
    }
}
