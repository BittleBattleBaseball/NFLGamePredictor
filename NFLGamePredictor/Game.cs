namespace NFLGamePredictor
{
    public class Game
    {
       public double WinnerFavoredBy { get
            {
                if(HomeTeam.WinProbability > AwayTeam.WinProbability)
                {
                    return HomeTeam.WinProbability;
                }

                return AwayTeam.WinProbability;
            }
        }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }
    }
}
