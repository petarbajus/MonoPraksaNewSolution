using Microsoft.AspNetCore.Mvc;
using NewSolution.Model;
using NewSolution.Service;

namespace NewSolution.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubController : ControllerBase
    {
        [HttpPost("insertClub")]
        public async Task<IActionResult>  InsertClubAsync([FromBody] Club club)
        {
            var clubService = new ClubService();

            if (club == null)
            {
                return BadRequest("Club object is null.");
            }

            if (string.IsNullOrEmpty(club.Name))
            {
                return BadRequest("Club name is missing.");
            }

            var success = await clubService.InsertClubAsync(club);
            return success ? Ok() : BadRequest("Failed to insert club.");
        }

        [HttpDelete("deleteClubById/{id}")]
        public async Task<IActionResult> DeleteClubByIdAsync(Guid id)
        {
            var clubService = new ClubService();

            var success = await clubService.DeleteClubByIdAsync(id);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpPut("updateClubById/{id}")]
        public async Task<IActionResult> UpdateClubByIdAsync(Guid id, [FromBody] Club club)
        {
            var clubService = new ClubService();

            var success = await clubService.UpdateClubByIdAsync(id, club);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpGet("getClubById/{id}")]
        public async Task<IActionResult> GetClubByIdAsync(Guid id)
        {
            var clubService = new ClubService();

            var club = await clubService.GetClubByIdAsync(id);
            return club != null ? Ok(club) : NotFound("Club not found.");
        }

        [HttpGet("getClubs")]
        public async Task<IActionResult> GetClubsAsync()
        {
            var clubService = new ClubService();

            var clubs = await clubService.GetClubsAsync();
            return  clubs != null && clubs.Any() ? Ok(clubs) : NotFound("No clubs found.");
        }
    }
}
