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

            // Adjust for Yards Per game
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

            // Adjust for Points Per game
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
             
             // Adjust for Third Down Converted Pct
            if (this.HomeTeam.Stats.ThirdDownConvertedPct > this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp +=  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp -= this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
            }
            else if (this.HomeTeam.Stats.ThirdDownConvertedPct < this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp -= this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp +=  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
            }

            /*
             
             // Adjust for QBR
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

            // Adjust for Total Touchdowns
            if (this.HomeTeam.Stats.Touchdowns > this.AwayTeam.Stats.Touchdowns)
            {
                homeAdjWp += this.HomeTeam.Stats.Touchdowns / this.AwayTeam.Stats.Touchdowns;
                awayAdjWp -= this.HomeTeam.Stats.Touchdowns / this.AwayTeam.Stats.Touchdowns;
            }
            else if (this.HomeTeam.Stats.Touchdowns < this.AwayTeam.Stats.Touchdowns)
            {
                homeAdjWp -= this.HomeTeam.Stats.Touchdowns / this.AwayTeam.Stats.Touchdowns;
                awayAdjWp += this.HomeTeam.Stats.Touchdowns / this.AwayTeam.Stats.Touchdowns;
            }

              // Adjust for Yards per pass attempt which since 2000 has predicted the winner 75% of the time
            if (this.HomeTeam.Stats.YardsPerPassAttempt > this.AwayTeam.Stats.YardsPerPassAttempt)
            {
                homeAdjWp += this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
                awayAdjWp -= this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
            }
            else if (this.HomeTeam.Stats.YardsPerPassAttempt < this.AwayTeam.Stats.YardsPerPassAttempt)
            {
                homeAdjWp -= this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
                awayAdjWp += this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
            }
             
              // Adjust for sacks FOR
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

            // Adjust Against sacks Against
            if (this.HomeTeam.Stats.SacksAgainst < this.AwayTeam.Stats.SacksAgainst)
            {
                homeAdjWp += 1.0;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
                awayAdjWp -= 1.0;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
            }
            else if (this.HomeTeam.Stats.SacksAgainst > this.AwayTeam.Stats.SacksAgainst)
            {
                homeAdjWp -= 1.0;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
                awayAdjWp += 1.0;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
            }
            

            // Adjust for Total Time Of Posession
            if (this.HomeTeam.Stats.TimeOfpossessionInSeconds > this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp += 1.5;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
                awayAdjWp -= 1.5;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
            }
            else if (this.HomeTeam.Stats.TimeOfpossessionInSeconds < this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp -= 1.5;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
                awayAdjWp += 1.5;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
            }

            // Adjust for Turover Differential Takeaways - Giveaways
            if (this.HomeTeam.Stats.TurnOverDifferential > this.AwayTeam.Stats.TurnOverDifferential)
            {
                homeAdjWp += 1.5;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
                awayAdjWp -= 1.5;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
            }
            else if (this.HomeTeam.Stats.TurnOverDifferential < this.AwayTeam.Stats.TurnOverDifferential)
            {
                homeAdjWp -= 1.5;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
                awayAdjWp += 1.5;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
            }

             // Adjust for Defensive Stuffs
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

            */

            this.HomeTeam.AdjustedWinProbability = homeAdjWp;
            this.AwayTeam.AdjustedWinProbability = awayAdjWp;

        }
    }
}
