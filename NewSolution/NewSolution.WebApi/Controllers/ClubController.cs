using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service.Common;
using NewSolution.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Net.NetworkInformation;
using NewSolution.WebApi.RestModels;

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
        public async Task<IActionResult> InsertClubAsync([FromBody] ClubAddModel clubAddModel)
        {
            if (clubAddModel == null || string.IsNullOrEmpty(clubAddModel.Name))
            {
                return BadRequest("Club name is required.");
            }

            var club = new Club
            {
                Name = clubAddModel.Name,
                CharacteristicColor = clubAddModel.CharacteristicColor,
                FoundationDate = clubAddModel.FoundationDate
            };

            var isSuccessful = await _clubService.InsertClubAsync(club);

            var clubGetModel = new ClubGetModel
            {
                Id = club.Id,
                Name = club.Name,
                CharacteristicColor = club.CharacteristicColor,
                FoundationDate = club.FoundationDate,
                //footballerNames = club.Footballers?.Select(f => f.Name).ToList() 
            };


            return isSuccessful ? Ok(clubGetModel) : BadRequest("Failed to insert club.");
        }

        [HttpDelete("deleteClubById/{id}")]
        public async Task<IActionResult> DeleteClubByIdAsync(Guid id)
        {

            var success = await _clubService.DeleteClubByIdAsync(id);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpPut("updateClubById/{id}")]
        public async Task<IActionResult> UpdateClubByIdAsync( [Required] Guid id, [FromBody] ClubUpdateModel clubUpdateModel)
        {
            if (clubUpdateModel == null)
            {
                return BadRequest("Club object is null.");
            }

            var club = new Club();
            club.Name = clubUpdateModel.Name;
            club.CharacteristicColor = clubUpdateModel.CharacteristicColor;
            club.FoundationDate = clubUpdateModel.FoundationDate;


            var success = await _clubService.UpdateClubByIdAsync(id, club);
            return success ? Ok("Club Updated") : NotFound("Failed to update club");
        }

        [HttpGet("getClubById/{id}")]
        public async Task<IActionResult> GetClubByIdAsync([Required] Guid id)
        {
           
            var club = await _clubService.GetClubByIdAsync(id);

            var clubGetModel = new ClubGetModel
            {
                Name = club.Name,
                CharacteristicColor = club.CharacteristicColor,
                FoundationDate = club.FoundationDate,
                //footballerNames = club.Footballers?.Select(f => f.Name).ToList() 
            };


            return clubGetModel != null ? Ok(clubGetModel) : NotFound("Club not found.");
        }

        [HttpGet("getClubs")]
        public async Task<IActionResult> GetClubsAsync(string searchQuery = "", string characteristicColor = "", DateOnly? dateOfFoundationFrom = null, DateOnly? dateOfFoundationTo = null,
            string sortBy = "ClubId", string sortDirection = "DESC", int recordsPerPage = 10, int currentPage = 1)
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

            var clubGetModels = clubs.Select(c => new ClubGetModel
            {
                Id = c.Id,
                Name = c.Name,
                CharacteristicColor = c.CharacteristicColor,
                FoundationDate = c.FoundationDate,
                //footballerNames = c.Footballers?.Select(f => f.Name).ToList()  // Adding footballer names
            }).ToList();

            return clubGetModels != null && clubGetModels.Any() ? Ok(clubGetModels) : NotFound("No clubs found.");
        }
    }
}
