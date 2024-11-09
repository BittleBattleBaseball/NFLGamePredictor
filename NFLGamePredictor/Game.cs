using NFLGamePredictor.Dto;

namespace NFLGamePredictor
{
    public class Game
    {
        private const double _homeFieldAdvantagePctIncrease = .25;

        private const double _adj = .25;

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
            int divider = 4;
            var homeAdjWp = this.HomeTeam.WinProbability > 50.0 ? ((this.HomeTeam.WinProbability - 50.0) / divider) + 50.0 : 50.0 - ((50.0 - this.HomeTeam.WinProbability) / divider);
            var awayAdjWp = this.AwayTeam.WinProbability > 50.0 ? ((this.AwayTeam.WinProbability - 50.0) / divider) + 50.0 : 50.0 - ((50.0 - this.AwayTeam.WinProbability) / divider);

            if(homeAdjWp > awayAdjWp)
                awayAdjWp = 100 - homeAdjWp;
            else if (awayAdjWp > homeAdjWp)
                homeAdjWp = 100 - awayAdjWp;

            //1) Adjust for homefield advantage
            homeAdjWp = homeAdjWp + _homeFieldAdvantagePctIncrease;
            awayAdjWp = awayAdjWp - _homeFieldAdvantagePctIncrease;

            // Adjust for Points Per game
            if (this.HomeTeam.Stats.PointsPerGame > this.AwayTeam.Stats.PointsPerGame)
            {
                var adjuster = this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
                homeAdjWp += _adj * adjuster;
                awayAdjWp -= _adj *  adjuster;
            }
            else if (this.HomeTeam.Stats.PointsPerGame < this.AwayTeam.Stats.PointsPerGame)
            {
                var adjuster = this.AwayTeam.Stats.PointsPerGame / this.HomeTeam.Stats.PointsPerGame;
                homeAdjWp -= _adj * adjuster;
                awayAdjWp += _adj * adjuster;
            }

            // Adjust for QBR
            if (this.HomeTeam.Stats.QBRating > this.AwayTeam.Stats.QBRating)
            {
                var adjuster = this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating;
                homeAdjWp += _adj * adjuster;
                awayAdjWp -= _adj * adjuster;
            }
            else if (this.HomeTeam.Stats.QBRating < this.AwayTeam.Stats.QBRating)
            {
                var adjuster = this.AwayTeam.Stats.QBRating / this.HomeTeam.Stats.QBRating;
                homeAdjWp -= _adj * adjuster;
                awayAdjWp += _adj *  adjuster;
            }

            // Adjust for Total Touchdowns
            if (this.HomeTeam.Stats.Touchdowns > this.AwayTeam.Stats.Touchdowns)
            {
                var adjuster = this.HomeTeam.Stats.Touchdowns / this.AwayTeam.Stats.Touchdowns;
                homeAdjWp += _adj * adjuster;
                awayAdjWp -= _adj * adjuster;
            }
            else if (this.HomeTeam.Stats.Touchdowns < this.AwayTeam.Stats.Touchdowns)
            {
                var adjuster = this.AwayTeam.Stats.Touchdowns / this.HomeTeam.Stats.Touchdowns;
                homeAdjWp -= _adj * adjuster;
                awayAdjWp += _adj * adjuster;
            }

            /*
            // Adjust for Total Time Of Posession
            if (this.HomeTeam.Stats.TimeOfpossessionInSeconds > this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp += _adj;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
                awayAdjWp -= _adj;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
            }
            else if (this.HomeTeam.Stats.TimeOfpossessionInSeconds < this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp -= _adj;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
                awayAdjWp += _adj;// this.HomeTeam.Stats.TimeOfpossessionInSeconds / this.AwayTeam.Stats.TimeOfpossessionInSeconds;
            }

            // Adjust for Yards Per game
            if (this.HomeTeam.Stats.YardsPerGame > this.AwayTeam.Stats.YardsPerGame)
            {
                homeAdjWp += _adj *  this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
                awayAdjWp -= _adj * this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
            }
            else if (this.HomeTeam.Stats.YardsPerGame < this.AwayTeam.Stats.YardsPerGame)
            {
                homeAdjWp -= _adj * this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
                awayAdjWp += _adj *  this.HomeTeam.Stats.YardsPerGame / this.AwayTeam.Stats.YardsPerGame;
            }

             // Adjust for Third Down Converted Pct
            if (this.HomeTeam.Stats.ThirdDownConvertedPct > this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp += _adj *  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp -= _adj *  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
            }
            else if (this.HomeTeam.Stats.ThirdDownConvertedPct < this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp -= _adj *  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp += _adj *  this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
            }



            // Adjust for Yards per pass attempt which since 2000 has predicted the winner 75% of the time
            if (this.HomeTeam.Stats.YardsPerPassAttempt > this.AwayTeam.Stats.YardsPerPassAttempt)
            {
                homeAdjWp += _adj *  this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
                awayAdjWp -= _adj * this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
            }
            else if (this.HomeTeam.Stats.YardsPerPassAttempt < this.AwayTeam.Stats.YardsPerPassAttempt)
            {
                homeAdjWp -= _adj * this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
                awayAdjWp += _adj * this.HomeTeam.Stats.YardsPerPassAttempt / this.AwayTeam.Stats.YardsPerPassAttempt;
            }

              // Adjust for sacks FOR
            if (this.HomeTeam.Stats.SacksFor > this.AwayTeam.Stats.SacksFor)
            {
                homeAdjWp += _adj;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
                awayAdjWp -= _adj;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
            }
            else if (this.HomeTeam.Stats.SacksFor < this.AwayTeam.Stats.SacksFor)
            {
                homeAdjWp -= _adj;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
                awayAdjWp += _adj;// this.HomeTeam.Stats.SacksFor / this.AwayTeam.Stats.SacksFor;
            }

            // Adjust Against sacks Against
            if (this.HomeTeam.Stats.SacksAgainst < this.AwayTeam.Stats.SacksAgainst)
            {
                homeAdjWp += _adj;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
                awayAdjWp -= _adj;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
            }
            else if (this.HomeTeam.Stats.SacksAgainst > this.AwayTeam.Stats.SacksAgainst)
            {
                homeAdjWp -= _adj;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
                awayAdjWp += _adj;// this.HomeTeam.Stats.SacksAgainst / this.AwayTeam.Stats.SacksAgainst;
            }


            // Adjust for Turover Differential Takeaways - Giveaways
            if (this.HomeTeam.Stats.TurnOverDifferential > this.AwayTeam.Stats.TurnOverDifferential)
            {
                homeAdjWp += _adj;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
                awayAdjWp -= _adj;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
            }
            else if (this.HomeTeam.Stats.TurnOverDifferential < this.AwayTeam.Stats.TurnOverDifferential)
            {
                homeAdjWp -= _adj;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
                awayAdjWp += _adj;// this.HomeTeam.Stats.TurnOverDifferential / this.AwayTeam.Stats.TurnOverDifferential;
            }

             // Adjust for Defensive Stuffs
            if (this.HomeTeam.Stats.DefensiveStuffs > this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp += _adj * this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
                awayAdjWp -= _adj * this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
            }
            else if (this.HomeTeam.Stats.DefensiveStuffs < this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp -= _adj * this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
                awayAdjWp += _adj *  this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
            }


         */

            this.HomeTeam.AdjustedWinProbability = homeAdjWp;
            this.AwayTeam.AdjustedWinProbability = awayAdjWp;

        }
    }
}
