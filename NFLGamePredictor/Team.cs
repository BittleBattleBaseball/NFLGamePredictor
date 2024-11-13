namespace NFLGamePredictor
{
    public class Team
    {
        public double WeeklyRank { get; set; }

        public string Name { get; set; }

        public double AdjustedWinProbability { get; set; }

        public double WinProbability { get; set; }

        public double MatchupQuality { get; set; }

        public bool IsHomeTeam { get; set; }

        public double EspnBETOddsSpread { get; set; }

        //public double TeamChanceLoss { get; set; }

        public double TeamPredPtDiff { get; set; }

        public int EspnOddsRanking { get; set; }    

        public int EspnPctProbabilityRanking { get; set; }      

        public double TotalCombinedConfidence
        {
            get
            {
                //return this.WinProbability + (this.EspnBETOddsSpread * -1);
                return this.EspnOddsRanking + this.EspnPctProbabilityRanking;
            }
        }

        //***NEW STATS
        public TeamStats Stats { get; set; }
    }
}
