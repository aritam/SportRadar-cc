using Microsoft.AspNetCore.Mvc;
using SportRadar.API.Models;
using SportRadar.API.Services.Interfaces;
using SportRadar.API.Utilities.Interfaces;
using System.Text;

namespace SportRadar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerDatasController : ControllerBase
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly ISerializer _csvSerializer;

        public PlayerDatasController(IPlayerDataService playerDataService, ISerializer csvSerializer)
        {
            _playerDataService = playerDataService;
            _csvSerializer = csvSerializer;
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAndSeason([FromQuery]long id, [FromQuery]int season)
        {
            PlayerData playerData = await _playerDataService.GetPlayerDataByIdAndSeason(id, season);

            if (playerData is not null && playerData.Name is not null)
            {
                string fileName = $"{playerData.Name}-{season}.csv";
                string playerDataCsv = await _csvSerializer.SerializeAsync(playerData);
                byte[] fileBytes = Encoding.ASCII.GetBytes(playerDataCsv);
                return File(fileBytes, "text/csv", fileName);
            }

            return NotFound();
        }
    }
}
