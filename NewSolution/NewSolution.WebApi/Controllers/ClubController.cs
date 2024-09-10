﻿using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service;
using NewSolution.Service.Common;
using System.ComponentModel.DataAnnotations;

namespace NewSolution.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubController : ControllerBase
    {
        private IClubService _clubService;
        
        public ClubController(IClubService clubService) { 
            _clubService = clubService;
        }

        [HttpPost("insertClub")]
        public async Task<IActionResult>  InsertClubAsync([FromBody] Club club)
        {

            if (club == null)
            {
                return BadRequest("Club object is null.");
            }

            if (string.IsNullOrEmpty(club.Name))
            {
                return BadRequest("Club name is missing.");
            }

            var success = await _clubService.InsertClubAsync(club);
            return success ? Ok() : BadRequest("Failed to insert club.");
        }

        [HttpDelete("deleteClubById/{id}")]
        public async Task<IActionResult> DeleteClubByIdAsync(Guid id)
        {

            var success = await _clubService.DeleteClubByIdAsync(id);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpPut("updateClubById/{id}")]
        public async Task<IActionResult> UpdateClubByIdAsync( [Required] Guid id, [FromBody] Club club)
        {

            var success = await _clubService.UpdateClubByIdAsync(id, club);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpGet("getClubById/{id}")]
        public async Task<IActionResult> GetClubByIdAsync([Required] Guid id)
        {
           
            var club = await _clubService.GetClubByIdAsync(id);
            return club != null ? Ok(club) : NotFound("Club not found.");
        }

        [HttpGet("getClubs")]
        public async Task<IActionResult> GetClubsAsync()
        {
            
            var clubs = await _clubService.GetClubsAsync();
            return  clubs != null && clubs.Any() ? Ok(clubs) : NotFound("No clubs found.");
        }
    }
}
