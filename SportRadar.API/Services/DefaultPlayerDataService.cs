using Microsoft.Extensions.Options;
using SportRadar.API.Configuration;
using SportRadar.API.Models;
using SportRadar.API.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SportRadar.API.Services
{
    public class DefaultPlayerDataService : IPlayerDataService
    {

        private readonly HttpClient _httpClient;
        private readonly ApiOptions _apiOptions;

        public DefaultPlayerDataService(HttpClient httpClient, IOptions<ApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiOptions = apiOptions.Value;
        }


        public async Task<PlayerData?> GetPlayerDataByIdAndSeason(long id, int season)
        {

            var specificPlayerEndpoint = $"{_apiOptions.PlayerEndpoint}{id}";
            var playerResponse = await _httpClient.GetAsync(specificPlayerEndpoint);

            if (playerResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            JsonObject playerApiJsonObject = await JsonSerializer.DeserializeAsync<JsonObject>(await playerResponse.Content.ReadAsStreamAsync());

            if (playerApiJsonObject is null)
            {
                return null;
            }

            var specificPlayerStatsEndpoint = $"{_apiOptions.PlayerEndpoint}{id}/stats?stats=statsSingleSeason&season={season}";
            var playerStatsResponse = await _httpClient.GetAsync(specificPlayerStatsEndpoint);

            if (playerStatsResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            JsonObject playerStatsApiJsonObject = await JsonSerializer.DeserializeAsync<JsonObject>(await playerStatsResponse.Content.ReadAsStreamAsync());

            if (playerStatsApiJsonObject is null)
            {
                return null;
            }

            try
            {
                PlayerData playerData = new PlayerData
                {
                    Id = (long)playerApiJsonObject["people"][0]["id"],
                    Name = (string)playerApiJsonObject["people"][0]["fullName"],
                    Team = (string)playerApiJsonObject["people"][0]["currentTeam"]["name"],
                    Age = (int)playerApiJsonObject["people"][0]["currentAge"],
                    Number = (string)playerApiJsonObject["people"][0]["primaryNumber"],
                    Position = (string)playerApiJsonObject["people"][0]["primaryPosition"]["name"],
                    IsRookie = (bool)playerApiJsonObject["people"][0]["rookie"],
                    Assists = (int)playerStatsApiJsonObject["stats"][0]["splits"][0]["stat"]["assists"],
                    Goals = (int)playerStatsApiJsonObject["stats"][0]["splits"][0]["stat"]["goals"],
                    Games = (int)playerStatsApiJsonObject["stats"][0]["splits"][0]["stat"]["games"],
                    Hits = (int)playerStatsApiJsonObject["stats"][0]["splits"][0]["stat"]["hits"],
                    Points = (int)playerStatsApiJsonObject["stats"][0]["splits"][0]["stat"]["points"]
                };

                return playerData;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
