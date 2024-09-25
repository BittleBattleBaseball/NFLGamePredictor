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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://nfl-api-data.p.rapidapi.com/nfl-weeks-events?year={year}&week={week}&type=2");

                request.Headers.Add("x-rapidapi-host", "nfl-api-data.p.rapidapi.com");
                request.Headers.Add("x-rapidapi-key", "af5352e3e5msh027e7a5c8c8cc76p157788jsndab27210c9c4");

                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    GamesByWeekResponse? gamesByWeekResponse = JsonConvert.DeserializeObject<GamesByWeekResponse>(responseBody);

                    if (gamesByWeekResponse != null)
                    {
                        foreach (var game in gamesByWeekResponse.items)
                        {
                            GamePredictorResponse gamePredictorResponse = await GetPrediction(Convert.ToInt32(game.eventid));
                            predictions.Add(ConvertPrediction(gamePredictorResponse));
                        }
                    }
                }
            }

            return predictions.OrderByDescending(g => g.WinnerFavoredBy).ToList();

        }

        private Game ConvertPrediction(GamePredictorResponse gamePredictorResponse)
        {
            return new Game
            {
                HomeTeam = new Team
                {
                    Name = gamePredictorResponse.homeTeam.team.displayName,
                    WinProbability = gamePredictorResponse.homeTeam.statistics[0].value
                },
                AwayTeam = new Team
                {
                    Name = gamePredictorResponse.awayTeam.team.displayName,
                    WinProbability = gamePredictorResponse.awayTeam.statistics[0].value
                }
            };
        }

        private async Task<GamePredictorResponse> GetPrediction(int gameId)
        {
            GamePredictorResponse gamePredictorResponse = null;
            using (HttpClient client = new HttpClient())
            {

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://nfl-api-data.p.rapidapi.com/nfl-predictor?id={gameId}");

                request.Headers.Add("x-rapidapi-host", "nfl-api-data.p.rapidapi.com");
                request.Headers.Add("x-rapidapi-key", "af5352e3e5msh027e7a5c8c8cc76p157788jsndab27210c9c4");

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    gamePredictorResponse = JsonConvert.DeserializeObject<GamePredictorResponse>(responseBody);
                }

            }

            return gamePredictorResponse;
        }


    }
}
