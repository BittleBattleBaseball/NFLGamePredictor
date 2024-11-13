using NFLGamePredictor.Dto;

namespace NFLGamePredictor.Contracts
{
    public interface IGamePredictorService
    {
        Task<List<Game>> GetPredictions(int year, int week);

        Task<List<Team>> GetWeeklyTeamRankings(int year, int week);
    }
}
