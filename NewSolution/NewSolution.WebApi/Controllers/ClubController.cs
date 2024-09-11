using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service.Common;
using NewSolution.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Net.NetworkInformation;

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
        public async Task<IActionResult> GetClubsAsync(string searchQuery = "", string characteristicColor = "", DateOnly? dateOfFoundationFrom = null, DateOnly? dateOfFoundationTo = null,
            string sortBy = "", string sortDirection = "", int recordsPerPage = 10, int currentPage = 1)
        {

            ClubFilter clubFilter = new ClubFilter
            {
                SearchQuery = searchQuery,
                CharacteristicColor = characteristicColor,
                DateOfFoundationFrom = dateOfFoundationFrom,
                DateOfFoundationTo = dateOfFoundationTo
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

            var clubs = await _clubService.GetClubsAsync(clubFilter, paging, sorting);
            return  clubs != null && clubs.Any() ? Ok(clubs) : NotFound("No clubs found.");
        }
    }
}
