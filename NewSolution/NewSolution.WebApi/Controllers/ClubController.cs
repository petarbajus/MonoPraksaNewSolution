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
        public IActionResult InsertClub([FromBody] Club club)
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

            var success = clubService.InsertClub(club);
            return success ? Ok() : BadRequest("Failed to insert club.");
        }

        [HttpDelete("deleteClubById/{id}")]
        public IActionResult DeleteClubById(Guid id)
        {
            var clubService = new ClubService();

            var success = clubService.DeleteClubById(id);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpPut("updateClubById/{id}")]
        public IActionResult UpdateClubById(Guid id, [FromBody] Club club)
        {
            var clubService = new ClubService();

            var success = clubService.UpdateClubById(id, club);
            return success ? Ok() : NotFound("Club not found.");
        }

        [HttpGet("getClubById/{id}")]
        public IActionResult GetClubById(Guid id)
        {
            var clubService = new ClubService();

            var club = clubService.GetClubById(id);
            return club != null ? Ok(club) : NotFound("Club not found.");
        }

        [HttpGet("getClubs")]
        public IActionResult GetClubs()
        {
            var clubService = new ClubService();

            var clubs = clubService.GetClubs();
            return clubs != null && clubs.Any() ? Ok(clubs) : NotFound("No clubs found.");
        }
    }
}
