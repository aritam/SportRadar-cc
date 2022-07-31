using SportRadar.API.Models;

namespace SportRadar.API.Services.Interfaces
{
    public interface ITeamDataService
    {
        public Task<TeamData> GetTeamDataByIdAndSeason(int id, int season);
    }
}
