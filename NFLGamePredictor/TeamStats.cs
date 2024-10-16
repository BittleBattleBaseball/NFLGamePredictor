namespace NFLGamePredictor
{
    public class TeamStats
    {
        private double totalTimeOfpossessionInSeconds;
        private double totaTouchdowns;
        private double sacksFor;
        private double sacksAgainst;
        private double defensiveStuffs;
        private double turnOverDifferential;

        public double GamesPlayed { get; set; }

        public double YardsPerGame { get; set; }

        public double PointsPerGame { get; set; }

        //public int PointsFor { get; set; }

        //public int PointsAgainst { get; set; }

        //public int TotalYardsFor { get; set; }

        //public int RusshingAttempts { get; set; }

        public double SacksFor { get => sacksFor / GamesPlayed; set => sacksFor = value; }

        public double SacksAgainst { get => sacksAgainst / GamesPlayed; set => sacksAgainst = value; }

        //public int TouchdownsFor { get; set; }

        //public int TouchdownsAgainst { get; set; }

        //public int TotalYardsAgainst { get; set; }

        //public int SeasonWins { get; set; }

        //public int SeasonLosses { get; set; }

        public double DefensiveStuffs { get => defensiveStuffs / GamesPlayed; set => defensiveStuffs = value; }

        public double QBRating { get; set; }

        public double TimeOfpossessionInSeconds { get => totalTimeOfpossessionInSeconds / GamesPlayed; set => totalTimeOfpossessionInSeconds = value; }

        public double ThirdDownConvertedPct { get; set; }

        public double Touchdowns { get => totaTouchdowns / GamesPlayed; set => totaTouchdowns = value; }

        public double YardsPerPassAttempt { get; set; }

        public double TurnOverDifferential { get => turnOverDifferential / GamesPlayed; set => turnOverDifferential = value; }
    }
}
