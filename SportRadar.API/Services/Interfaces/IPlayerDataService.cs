using SportRadar.API.Models;

namespace SportRadar.API.Services.Interfaces
{
    public interface IPlayerDataService
    {
        public Task<PlayerData> GetPlayerDataByIdAndSeason(long id, int season);
    }
}
