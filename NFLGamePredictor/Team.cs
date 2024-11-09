namespace NFLGamePredictor
{
    public class Team
    {
        public string Name { get; set; }

        public double AdjustedWinProbability { get; set; }

        public double WinProbability { get; set; }

        public double MatchupQuality { get; set; }

        public bool IsHomeTeam { get; set; }

        public double EspnBETOddsSpread { get; set; }

        //public double TeamChanceLoss { get; set; }

        public double TeamPredPtDiff { get; set; }

        //***NEW STATS
        public TeamStats Stats { get; set; }
    }
}
