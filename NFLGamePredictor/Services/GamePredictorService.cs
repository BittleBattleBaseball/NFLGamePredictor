using Newtonsoft.Json;
using NFLGamePredictor.Contracts;
using NFLGamePredictor.Dto;

namespace NFLGamePredictor.Services
{
    public class GamePredictorService : IGamePredictorService
    {
        public async Task<List<Game>> GetPredictions(int year, int week)
        {
            var predictions = new List<Game>();

            using (HttpClient client = new HttpClient())
            {
                string responseBody = await client.GetStringAsync($"https://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/{year}/types/2/weeks/{week}/events");
                responseBody = responseBody.Replace("$ref", "gameUrl");
                GamesByWeekResponse? gamesByWeekResponse = JsonConvert.DeserializeObject<GamesByWeekResponse>(responseBody);

                if (gamesByWeekResponse != null)
                {
                    foreach (var game in gamesByWeekResponse.items)
                    {
                        string gameResponseBody = await client.GetStringAsync(game.gameUrl);
                        gameResponseBody = gameResponseBody.Replace("$ref", "id");
                        EventResponse? eventResponse = JsonConvert.DeserializeObject<EventResponse>(gameResponseBody);

                        string predictionResponse = await client.GetStringAsync($"http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/events/{eventResponse?.id}/competitions/{eventResponse?.id}/predictor");
                        predictionResponse = predictionResponse.Replace("$ref", "id");
                        GamePredictionResponse? gamePredictionResponse = JsonConvert.DeserializeObject<GamePredictionResponse>(predictionResponse);

                        string oddsRsp = await client.GetStringAsync($"http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/events/{eventResponse?.id}/competitions/{eventResponse?.id}/odds");
                        oddsRsp = oddsRsp.Replace("$ref", "id");
                        GameOddsResponse? gameOddsResponse = JsonConvert.DeserializeObject<GameOddsResponse>(oddsRsp);

                        predictions.Add(await ConvertPrediction(year, gamePredictionResponse, gameOddsResponse));
                    }
                }


            }

            List<Game> finalResults = new List<Game>();

            int confidencePoints = 16;
            foreach (var prediction in predictions.OrderByDescending(g => g.WinnerFavoredBy).ToList())
            {
                prediction.PenneckConfidencePoints = confidencePoints;
                finalResults.Add(prediction);
                confidencePoints--;
            }

            //int oddsConfidencePoints = 16;
            //foreach (var finalResult in finalResults.OrderByDescending(g => g.OddsFavoredBy).ToList())
            //{
            //    finalResult.OddsConfidencePoints = oddsConfidencePoints;
            //    oddsConfidencePoints--;
            //}

            //int finalRankConfidencePoints = 16;
            //foreach (var finalResult in finalResults.OrderByDescending(g => g.TotalConfidencePoints).ToList())
            //{
            //    finalResult.PenneckConfidencePoints = finalRankConfidencePoints;
            //    finalRankConfidencePoints--;
            //}

            return finalResults;//.OrderByDescending(x => x.PenneckConfidencePoints).ToList();

        }

        private async Task<Game> ConvertPrediction(int season, GamePredictionResponse? gamePredictionResponse, GameOddsResponse? gameOddsResponse)
        {
            string[] teams = gamePredictionResponse.name.Split(" at ");
            var result = new Game
            {
                Name = gamePredictionResponse.name,             
                HomeTeam = new Team
                {
                    Name = teams[1].Trim(),
                    IsHomeTeam = true,
                    WinProbability = gamePredictionResponse.homeTeam.statistics[0].value,
                    MatchupQuality = gamePredictionResponse.homeTeam.statistics[1].value,
                    TeamPredPtDiff = gamePredictionResponse.homeTeam.statistics[6].value,
                    EspnBETOddsSpread = Convert.ToDouble(gameOddsResponse.items[0].homeTeamOdds.current.pointSpread.american),
                    Stats = await GetTeamStats(season, GetTeamNumberByName(teams[1].Trim()))
                },
                AwayTeam = new Team
                {
                    Name= teams[0].Trim(),
                    WinProbability = gamePredictionResponse.awayTeam.statistics[0].value,
                    MatchupQuality = gamePredictionResponse.awayTeam.statistics[1].value,
                    TeamPredPtDiff = gamePredictionResponse.awayTeam.statistics[6].value,
                    EspnBETOddsSpread = Convert.ToDouble(gameOddsResponse.items[0].awayTeamOdds.current.pointSpread.american),
                    Stats = await GetTeamStats(season, GetTeamNumberByName(teams[0].Trim()))
                }
            };

            //!!!! THIS IS WHERE I PUT MY OWN LOGIC IN BEYOND JUST ESPN ANALYTICS
            result.AdjustForOtherFactors();
            return result;
        }

        //private async Task<GamePredictorResponse> GetPrediction(int gameId)
        //{
        //    GamePredictorResponse gamePredictorResponse = null;
        //    using (HttpClient client = new HttpClient())
        //    {

        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://nfl-api-data.p.rapidapi.com/nfl-predictor?id={gameId}");

        //        request.Headers.Add("x-rapidapi-host", "nfl-api-data.p.rapidapi.com");
        //        request.Headers.Add("x-rapidapi-key", "af5352e3e5msh027e7a5c8c8cc76p157788jsndab27210c9c4");

        //        using (var response = await client.SendAsync(request))
        //        {
        //            response.EnsureSuccessStatusCode();
        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            gamePredictorResponse = JsonConvert.DeserializeObject<GamePredictorResponse>(responseBody);
        //        }

        //    }

        //    return gamePredictorResponse;
        //}

        private async Task<TeamStats> GetTeamStats(int season, int teamId)
        {
            using (HttpClient client = new HttpClient())
            {
                string responseBody = await client.GetStringAsync($"https://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/{season}/types/2/teams/{teamId}/statistics");

                responseBody = responseBody.Replace("$ref", "id");
                TeamStatsResponse? teamStats = JsonConvert.DeserializeObject<TeamStatsResponse>(responseBody);
                return ConvertResponseToTeamStats(teamStats);
            }
        }

        private static TeamStats ConvertResponseToTeamStats(TeamStatsResponse response)
        {
            TeamStats stats = new TeamStats();
            var generalStats = response.splits.categories[0];
            var passingStats = response.splits.categories[1];
            var rushingStats = response.splits.categories[2];
            var scoringStats = response.splits.categories[9];
            var defensiveStats = response.splits.categories[4];
            var miscStats = response.splits.categories[10];

            stats.GamesPlayed = generalStats.stats.FirstOrDefault(x => x.displayName == "Games Played").value;
            //stats.PointsAgainst = Convert.ToInt32(defensiveStats.stats[24].value);            
            //stats.PointsFor = Convert.ToInt32(scoringStats.stats[8].value);
            stats.SacksFor = defensiveStats.stats.FirstOrDefault(x => x.displayName == "Sacks").value;
            stats.SacksAgainst = passingStats.stats.FirstOrDefault(x => x.displayName == "Total Sacks").value;
            stats.YardsPerGame = passingStats.stats.FirstOrDefault(x => x.displayName == "Net Yards Per Game").value; //"Net Passing Yards Per Game"
            stats.PointsPerGame = scoringStats.stats.FirstOrDefault(x => x.displayName == "Total Points Per Game").value;//"Total Points Per Game"
            stats.DefensiveStuffs = rushingStats.stats.FirstOrDefault(x => x.displayName == "Stuffs").value;
            stats.QBRating = passingStats.stats.FirstOrDefault(x => x.displayName == "Quarterback Rating").value; //"Quarterback Rating"
            stats.TimeOfpossessionInSeconds = miscStats.stats.FirstOrDefault(x => x.displayName == "Possession Time Seconds").value; //"Possession Time Seconds"
            stats.ThirdDownConvertedPct = miscStats.stats.FirstOrDefault(x => x.displayName == "3rd down %").value;//"3rd down %"
            stats.Touchdowns = passingStats.stats.FirstOrDefault(x => x.displayName == "Total Touchdowns").value; //"Total Touchdowns"
            stats.YardsPerPassAttempt = passingStats.stats.FirstOrDefault(x => x.displayName == "Yards Per Pass Attempt").value; //"Yards Per Pass Attempt"
            stats.TurnOverDifferential = miscStats.stats.FirstOrDefault(x => x.displayName == "Turnover Ratio").value;

            return stats;
        }

        private static int GetTeamNumberByName (string name)
        {
            name = name.ToLower().Trim();

            if (name == "atlanta falcons")
                return 1;
            else if (name == "buffalo bills")
                return 2;
            else if (name == "chicago bears")
                return 3;
            else if (name == "cincinnati bengals")
                return 4;
            else if (name == "cleveland browns")
                return 5;
            else if (name == "dallas cowboys")
                return 6;
            else if (name == "denver broncos")
                return 7;
            else if (name == "detroit lions")
                return 8;
            else if (name == "green bay packers")
                return 9;
            else if (name == "tennessee titans")
                return 10;
            else if (name == "indianapolis colts")
                return 11;
            else if (name == "kansas city chiefs")
                return 12;       
            else if (name == "las vegas raiders")
                return 13;
            else if (name == "los angeles rams")
                return 14;
            else if (name == "miami dolphins")
                return 15;
            else if (name == "minnesota vikings")
                return 16;
            else if (name == "new england patriots")
                return 17;
            else if (name == "new orleans saints")
                return 18;
            else if (name == "new york giants")
                return 19;
            else if (name == "new york jets")
                return 20;
            else if (name == "philadelphia eagles")
                return 21;
            else if (name == "arizona cardinals")
                return 22;
            else if (name == "pittsburgh steelers")
                return 23;
            else if (name == "los angeles chargers")
                return 24;
            else if (name == "san francisco 49ers")
                return 25;
            else if (name == "seattle seahawks")
                return 26;
            else if (name == "tampa bay buccaneers")
                return 27;
            else if (name == "washington commanders")
                return 28;
            else if (name == "carolina panthers")
                return 29;
            else if (name == "jacksonville jaguars")
                return 30;
            else if (name == "baltimore ravens")
                return 33;
            else if (name == "houston texans")
                return 34;


            return -1;
        }

    }
}
