using SportRadar.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportRadar.Tests.Fixtures
{
    public static class TeamDatasFixture
    {

        public static TeamData GetTestTeamData() =>

            new TeamData
            {
                Id = 5,
                Name = "Pittsburgh Penguins",
                VenueName = "PPG Paints Arena",
                GamesPlayed = 82,
                Wins = 50,
                Losses = 21,
                Points = 111,
                GoalsPerGame = 3.39F,
                FirstGameDate = "2016-09-27",
                FirstOpponentName = "Detroit Red Wings"
            };


        public static string GetTestTeamCsv() => "Id,Name,VenueName,GamesPlayed,Wins,Losses,Points,GoalsPerGame,FirstGameDate,FirstOpponentName\r\n5,Pittsburgh Penguins,PPG Paints Arena,82,50,21,111,3.39,2016-09-27,Detroit Red Wings";
    }
}
