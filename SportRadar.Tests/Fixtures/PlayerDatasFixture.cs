using SportRadar.API.Models;
using System.Text;

namespace SportRadar.Tests.Fixtures
{
    public static class PlayerDatasFixture
    {

        public static PlayerData GetTestPlayerData() =>

            new PlayerData
            {
                Id = 8476792,
                Name = "Torey Krug",
                Team = "St. Louis Blues",
                Age = 31,
                Number = "47",
                Position = "Defenseman",
                IsRookie = false,
                Assists = 27,
                Goals = 12,
                Games = 78,
                Hits = 75,
                Points = 39
            };

        public static string GetTestPlayerCsv() => "Id,Name,Team,Age,Number,Position,IsRookie,Assists,Goals,Games,Hits,Points\r\n8476792,Torey Krug,St. Louis Blues,31,47,Defenseman,False,27,12,78,75,39";

    }
}
