using NFLGamePredictor.Dto;

namespace NFLGamePredictor.Contracts
{
    public interface IGamePredictorService
    {
        Task<List<Game>> GetPredictions(int year, int week);
    }
}
