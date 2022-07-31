using Microsoft.AspNetCore.Mvc;
using SportRadar.API.Models;
using SportRadar.API.Services.Interfaces;
using SportRadar.API.Utilities;
using SportRadar.API.Utilities.Interfaces;
using System.Text;

namespace SportRadar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamDatasController : ControllerBase
    {
        private readonly ITeamDataService _teamDataService;
        private readonly ISerializer _csvSerializer;

        public TeamDatasController(ITeamDataService teamDataService, ISerializer csvSerializer)
        {
            _teamDataService = teamDataService;
            _csvSerializer = csvSerializer;
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAndSeason([FromQuery]int id,[FromQuery]int season)
        {
            TeamData teamData = await _teamDataService.GetTeamDataByIdAndSeason(id, season);
            
            if (teamData is not null && teamData.Name is not null)
            {
                string fileName = $"{teamData.Name}-{season}.csv";
                string teamDataCsv = await _csvSerializer.SerializeAsync(teamData);
                byte[] fileBytes = Encoding.ASCII.GetBytes(teamDataCsv);

                return File(fileBytes, "text/csv", fileName);
            }

            return NotFound();
        }
    }
}
