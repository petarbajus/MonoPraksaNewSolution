using Microsoft.AspNetCore.Mvc;
using NewSolution.Common;
using NewSolution.Model;
using NewSolution.Service;
using NewSolution.Service.Common;
using NewSolution.WebApi.RestModels;
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
        public async Task<IActionResult> InsertFootballerAsync([FromBody] FootballerAddModel footballerAdd)
        {

            if (footballerAdd == null)
            {
                return BadRequest("Footballer object is null.");
            }

            var footballer = new Footballer();

            footballer.Name = footballerAdd.Name;
            footballer.DOB = footballerAdd.DOB;
            footballer.ClubId = footballerAdd.ClubId;



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
        public async Task<IActionResult> UpdateFootballerByIdAsync([Required] Guid id, [FromBody] FootballerUpdateModel footballerUpdateModel)
        {
            if (footballerUpdateModel == null)
            {
                return BadRequest("Footballer object is null.");
            }

            var footballer = new Footballer();

            footballer.Name = footballerUpdateModel.Name;
            footballer.ClubId = footballerUpdateModel.ClubId;

            var success = await _footballerService.UpdateFootballerByIdAsync(id, footballer);
            return success ? Ok() : NotFound("Footballer not found.");
        }

        [HttpGet("getFootballerById/{id}")]
        public async Task<IActionResult> GetFootballerByIdAsync([Required] Guid id)
        {
            var footballer = await _footballerService.GetFootballerByIdAsync(id);

            if (footballer == null)
            {
                return NotFound();
            }

            var footballerGetModel = new FootballerGetModel
            {
                Name = footballer.Name,
                DOB = footballer.DOB,
                ClubName = footballer.Club?.Name
            };
            return Ok(footballerGetModel); ;
        }

        [HttpGet("getFootballers")]
        public async Task<IActionResult> GetFootballersAsync(string searchQuery = "", DateOnly? DOBFrom = null, DateOnly? DOBTo = null,
            Guid? clubId = null, string sortBy = "FootballerId", string sortDirection = "DESC", int recordsPerPage = 10, int currentPage = 1)
        {
            FootballerFilter footballerFilter = new FootballerFilter
            {
                SearchQuery = searchQuery,
                DOBFrom = DOBFrom,
                DOBTo = DOBTo,
                ClubId = clubId
            };

            Sorting sorting = new Sorting
            {
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            Paging paging = new Paging
            {
                RecordsPerPage = recordsPerPage,
                CurrentPage = currentPage
            };

            var footballers = await _footballerService.GetFootballersAsync(footballerFilter, paging, sorting);

            var footballerGetModels = footballers.Select(f => new FootballerGetModel
            {
                Name = f.Name,
                DOB = f.DOB,
                ClubName = f.Club?.Name
            }).ToList();

            return footballerGetModels.Any() ? Ok(footballerGetModels) : NotFound("No footballers found.");
        }
    }
}
