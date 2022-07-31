﻿namespace SportRadar.API.Models
{
    public class PlayerData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public int Age { get; set; }
        public string Number { get; set; }
        public string Position { get; set; }
        public bool IsRookie { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int Games { get; set; }
        public int Hits { get; set; }
        public int Points { get; set; }
    }
}
