using NFLGamePredictor.Dto;

namespace NFLGamePredictor
{
    public class Game
    {
        private const double _homeFieldAdvantagePctIncrease = 2.5;

        public int ConfidencePoints { get; set; }

        public string Winner
        {
            get
            {
                if (HomeTeam.AdjustedWinProbability > AwayTeam.AdjustedWinProbability)
                {
                    return HomeTeam.Name;
                }

                return AwayTeam.Name;
            }
        }

        public double WinnerFavoredBy
        {
            get
            {
                if (HomeTeam.AdjustedWinProbability > AwayTeam.AdjustedWinProbability)
                {
                    return HomeTeam.AdjustedWinProbability - AwayTeam.AdjustedWinProbability;
                }

                return AwayTeam.AdjustedWinProbability - HomeTeam.AdjustedWinProbability;
            }
        }

        public string Name { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public bool IsDivisionGame { get; set; }

        public void AdjustForOtherFactors()
        {
            var homeAdjWp = this.HomeTeam.WinProbability;
            var awayAdjWp = this.AwayTeam.WinProbability;

            //1) Adjust for homefield advantage
            homeAdjWp = homeAdjWp + _homeFieldAdvantagePctIncrease;
            awayAdjWp = awayAdjWp - _homeFieldAdvantagePctIncrease;

            //2) Adjust for sacks FOR
            if (this.HomeTeam.Stats.SacksFor > this.AwayTeam.Stats.SacksFor)
            {
                homeAdjWp += 1.0;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
                awayAdjWp -= 1.0;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
            }
            else if (this.HomeTeam.Stats.SacksFor < this.AwayTeam.Stats.SacksFor)
            {
                homeAdjWp -= 1.0;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
                awayAdjWp += 1.0;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
            }

            //2) Adjust for Yards Per game
            if (this.HomeTeam.Stats.YardsPerGame > this.AwayTeam.Stats.YardsPerGame)
            {
                homeAdjWp += this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
                awayAdjWp -= this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
            }
            else if (this.HomeTeam.Stats.YardsPerGame < this.AwayTeam.Stats.YardsPerGame)
            {
                homeAdjWp -= this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
                awayAdjWp += this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
            }

            //3) Adjust for Points Per game
            if (this.HomeTeam.Stats.PointsPerGame > this.AwayTeam.Stats.PointsPerGame)
            {
                homeAdjWp += this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
                awayAdjWp -= this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
            }
            else if (this.HomeTeam.Stats.PointsPerGame < this.AwayTeam.Stats.PointsPerGame)
            {
                homeAdjWp -= this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
                awayAdjWp += this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
            }

            //4) Adjust for Defensive Stuffs
            if (this.HomeTeam.Stats.DefensiveStuffs > this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp += this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
                awayAdjWp -= this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
            }
            else if (this.HomeTeam.Stats.DefensiveStuffs < this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp -= this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
                awayAdjWp += this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
            }


            //5) Adjust for QBR
            if (this.HomeTeam.Stats.QBRating > this.AwayTeam.Stats.QBRating)
            {
                homeAdjWp += (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
                awayAdjWp -= (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
            }
            else if (this.HomeTeam.Stats.QBRating < this.AwayTeam.Stats.QBRating)
            {
                homeAdjWp -= (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
                awayAdjWp += (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
            }

            //6) Adjust for Total Time Of Posession
            if (this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds > this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds)
            {
                homeAdjWp += 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
                awayAdjWp -= 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
            }
            else if (this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds < this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds)
            {
                homeAdjWp -= 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
                awayAdjWp += 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
            }

            this.HomeTeam.AdjustedWinProbability = homeAdjWp;
            this.AwayTeam.AdjustedWinProbability = awayAdjWp;

        }
    }
}
