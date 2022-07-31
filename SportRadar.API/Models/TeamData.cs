

namespace SportRadar.API.Models
{
    public class TeamData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VenueName { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Points { get; set; }
        public float GoalsPerGame { get; set; }
        public string FirstGameDate { get; set; }
        public string FirstOpponentName { get; set; }

    }
}
