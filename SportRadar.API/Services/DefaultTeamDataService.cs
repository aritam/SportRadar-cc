using Microsoft.Extensions.Options;
using SportRadar.API.Configuration;
using SportRadar.API.Models;
using SportRadar.API.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SportRadar.API.Services
{
    public class DefaultTeamDataService : ITeamDataService
    {

        private readonly HttpClient _httpClient;
        private readonly ApiOptions _apiOptions;

        public DefaultTeamDataService(HttpClient httpClient, IOptions<ApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiOptions = apiOptions.Value;
        }

        public async Task<TeamData?> GetTeamDataByIdAndSeason(int id, int season)
        {
            var specificTeamEndpoint = $"{_apiOptions.TeamEndpoint}{id}?expand=team.stats&season={season}";
            var teamResponse = await _httpClient.GetAsync(specificTeamEndpoint);
            if (teamResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            JsonObject teamApiJsonObject = await JsonSerializer.DeserializeAsync<JsonObject>(await teamResponse.Content.ReadAsStreamAsync());

            if (teamApiJsonObject is null)
            {
                return null;
            }

            var specificScheduleEndpoint = $"{_apiOptions.ScheduleEndpoint}?teamId={id}&season={season}";
            var scheduleResponse = await _httpClient.GetAsync(specificScheduleEndpoint);
            if (scheduleResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            JsonObject scheduleApiJsonObject = await JsonSerializer.DeserializeAsync<JsonObject>(await scheduleResponse.Content.ReadAsStreamAsync());

            if (scheduleApiJsonObject is null)
            {
                return null;
            }

            string teamName = (string)teamApiJsonObject["teams"][0]["name"];
            string FirstGameAwayTeamName = (string)scheduleApiJsonObject["dates"][0]["games"][0]["teams"]["away"]["team"]["name"];
            string FirstGameHomeTeamName = (string)scheduleApiJsonObject["dates"][0]["games"][0]["teams"]["home"]["team"]["name"];

            try
            {
                TeamData teamData = new TeamData
                {
                    Id = (int)teamApiJsonObject["teams"][0]["id"],
                    Name = (string)teamApiJsonObject["teams"][0]["name"],
                    VenueName = (string)teamApiJsonObject["teams"][0]["venue"]["name"],
                    GamesPlayed = (int)teamApiJsonObject["teams"][0]["teamStats"][0]["splits"][0]["stat"]["gamesPlayed"],
                    Wins = (int)teamApiJsonObject["teams"][0]["teamStats"][0]["splits"][0]["stat"]["wins"],
                    Losses = (int)teamApiJsonObject["teams"][0]["teamStats"][0]["splits"][0]["stat"]["losses"],
                    Points = (int)teamApiJsonObject["teams"][0]["teamStats"][0]["splits"][0]["stat"]["pts"],
                    GoalsPerGame = (float)teamApiJsonObject["teams"][0]["teamStats"][0]["splits"][0]["stat"]["goalsPerGame"],
                    FirstGameDate = (string)scheduleApiJsonObject["dates"][0]["date"],
                    FirstOpponentName = teamName.Equals(FirstGameHomeTeamName) ? FirstGameAwayTeamName : FirstGameHomeTeamName
                };

                return teamData;
            }
            catch (NullReferenceException)
            {
                return null;
            }

        }
    }
}
