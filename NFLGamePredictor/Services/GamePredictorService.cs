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

                        predictions.Add(await ConvertPrediction(year, gamePredictionResponse));
                    }
                }


            }

            List<Game> finalResults = new List<Game>();

            int confidencePoints = 16;
           foreach(var prediction  in predictions.OrderByDescending(g => g.WinnerFavoredBy).ToList())
            {
                prediction.ConfidencePoints = confidencePoints;
                finalResults.Add(prediction);
                confidencePoints--;
            }

            return finalResults;

        }

        private async Task<Game> ConvertPrediction(int season, GamePredictionResponse? gamePredictionResponse)
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
                    Stats = await GetTeamStats(season, GetTeamNumberByName(teams[1].Trim()))
                },
                AwayTeam = new Team
                {
                    Name= teams[0].Trim(),
                    WinProbability = gamePredictionResponse.awayTeam.statistics[0].value,
                    MatchupQuality = gamePredictionResponse.awayTeam.statistics[1].value,
                    TeamPredPtDiff = gamePredictionResponse.awayTeam.statistics[6].value,
                    Stats = await GetTeamStats(season, GetTeamNumberByName(teams[0].Trim()))
                }
            };

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

            //stats.PointsAgainst = Convert.ToInt32(defensiveStats.stats[24].value);            
            //stats.PointsFor = Convert.ToInt32(scoringStats.stats[8].value);
            stats.SacksFor = Convert.ToInt32(defensiveStats.stats[14].value);
            stats.YardsPerGame = passingStats.stats[9].value;
            stats.PointsPerGame = scoringStats.stats[9].value;
            stats.DefensiveStuffs = rushingStats.stats[14].value;
            stats.QBRating = passingStats.stats[42].value;

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
