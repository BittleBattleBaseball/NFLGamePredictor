using NFLGamePredictor.Dto;

namespace NFLGamePredictor
{
    public class Game
    {
        private const double _homeFieldAdvantagePctIncrease = 2.5;

        private const double _adj = .25;

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

        public int PenneckConfidencePoints { get; set; }

        public int EspnPredictorConfidencePoints { get; set; }

        public double HomeTeamTotalConfidencePoints { get; set; }

        public double AwayTeamTotalConfidencePoints { get; set; }

        public double WinnerFavoredByCombinedPctAndOdds
        {
            get
            {
                if (HomeTeamTotalConfidencePoints > AwayTeamTotalConfidencePoints)
                {
                    return HomeTeam.AdjustedWinProbability - AwayTeam.AdjustedWinProbability;
                }

                return AwayTeam.AdjustedWinProbability - HomeTeam.AdjustedWinProbability;
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
            /*
            int divider = 4;
            var homeAdjWp = this.HomeTeam.WinProbability;// > 50.0 ? ((this.HomeTeam.WinProbability - 50.0) / divider) + 50.0 : 50.0 - ((50.0 - this.HomeTeam.WinProbability) / divider);
            var awayAdjWp = this.AwayTeam.WinProbability;// > 50.0 ? ((this.AwayTeam.WinProbability - 50.0) / divider) + 50.0 : 50.0 - ((50.0 - this.AwayTeam.WinProbability) / divider);

            
       if(homeAdjWp > awayAdjWp)
           awayAdjWp = 100 - homeAdjWp;
       else if (awayAdjWp > homeAdjWp)
           homeAdjWp = 100 - awayAdjWp;

       //1) Adjust for homefield advantage
       homeAdjWp = homeAdjWp + _homeFieldAdvantagePctIncrease;
       awayAdjWp = awayAdjWp - _homeFieldAdvantagePctIncrease;

       // Adjust for Odds Spreads
       if (this.HomeTeam.EspnBETOddsSpread < this.AwayTeam.EspnBETOddsSpread)//Negative is good
       {
           var adjuster = this.HomeTeam.EspnBETOddsSpread * -1 / 8;
           homeAdjWp +=  adjuster;
           awayAdjWp -=  adjuster;
       }
       else if (this.HomeTeam.EspnBETOddsSpread > this.AwayTeam.EspnBETOddsSpread)
       {
           var adjuster = this.AwayTeam.EspnBETOddsSpread * -1 / 8;
           homeAdjWp -=  adjuster;
           awayAdjWp +=  adjuster;
       }

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


    

            this.HomeTeam.AdjustedWinProbability = homeAdjWp;
            this.AwayTeam.AdjustedWinProbability = awayAdjWp;

            */


            //The below was my best 2 week stretch
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
                homeAdjWp -= this.AwayTeam.Stats.YardsPerGame / this.HomeTeam.Stats.YardsPerGame;
                awayAdjWp += this.AwayTeam.Stats.YardsPerGame / this.HomeTeam.Stats.YardsPerGame;
            }

            //3) Adjust for Points Per game
            if (this.HomeTeam.Stats.PointsPerGame > this.AwayTeam.Stats.PointsPerGame)
            {
                homeAdjWp += this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
                awayAdjWp -= this.HomeTeam.Stats.PointsPerGame / this.AwayTeam.Stats.PointsPerGame;
            }
            else if (this.HomeTeam.Stats.PointsPerGame < this.AwayTeam.Stats.PointsPerGame)
            {
                homeAdjWp -= this.AwayTeam.Stats.PointsPerGame / this.HomeTeam.Stats.PointsPerGame;
                awayAdjWp += this.AwayTeam.Stats.PointsPerGame / this.HomeTeam.Stats.PointsPerGame;
            }

            //4) Adjust for Defensive Stuffs
            if (this.HomeTeam.Stats.DefensiveStuffs > this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp += this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
                awayAdjWp -= this.HomeTeam.Stats.DefensiveStuffs / this.AwayTeam.Stats.DefensiveStuffs;
            }
            else if (this.HomeTeam.Stats.DefensiveStuffs < this.AwayTeam.Stats.DefensiveStuffs)
            {
                homeAdjWp -= this.AwayTeam.Stats.DefensiveStuffs / this.HomeTeam.Stats.DefensiveStuffs;
                awayAdjWp += this.AwayTeam.Stats.DefensiveStuffs / this.HomeTeam.Stats.DefensiveStuffs;
            }

            //5) Adjust for QBR
            if (this.HomeTeam.Stats.QBRating > this.AwayTeam.Stats.QBRating)
            {
                homeAdjWp += (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
                awayAdjWp -= (this.HomeTeam.Stats.QBRating / this.AwayTeam.Stats.QBRating) + 1.5;
            }
            else if (this.HomeTeam.Stats.QBRating < this.AwayTeam.Stats.QBRating)
            {
                homeAdjWp -= (this.AwayTeam.Stats.QBRating / this.HomeTeam.Stats.QBRating) + 1.5;
                awayAdjWp += (this.AwayTeam.Stats.QBRating / this.HomeTeam.Stats.QBRating) + 1.5;
            }

            //6) Adjust for Total Time Of Posession
            if (this.HomeTeam.Stats.TimeOfpossessionInSeconds > this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp += 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
                awayAdjWp -= 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
            }
            else if (this.HomeTeam.Stats.TimeOfpossessionInSeconds < this.AwayTeam.Stats.TimeOfpossessionInSeconds)
            {
                homeAdjWp -= 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
                awayAdjWp += 1.5;// this.HomeTeam.Stats.TotalTimeOfpossessionInSeconds / this.AwayTeam.Stats.TotalTimeOfpossessionInSeconds;
            }

            //7) Adjust for Third Down Converted Pct
            if (this.HomeTeam.Stats.ThirdDownConvertedPct > this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp += this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp -= this.HomeTeam.Stats.ThirdDownConvertedPct / this.AwayTeam.Stats.ThirdDownConvertedPct;
            }
            else if (this.HomeTeam.Stats.ThirdDownConvertedPct < this.AwayTeam.Stats.ThirdDownConvertedPct)
            {
                homeAdjWp -= this.AwayTeam.Stats.ThirdDownConvertedPct / this.HomeTeam.Stats.ThirdDownConvertedPct;
                awayAdjWp += this.AwayTeam.Stats.ThirdDownConvertedPct / this.HomeTeam.Stats.ThirdDownConvertedPct;
            }

            this.HomeTeam.AdjustedWinProbability = homeAdjWp;
            this.AwayTeam.AdjustedWinProbability = awayAdjWp;

        }
    }
}
