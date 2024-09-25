using Microsoft.AspNetCore.Mvc;
using NFLGamePredictor.Contracts;
using NFLGamePredictor.Dto;

namespace NFLGamePredictor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private IGamePredictorService _gamePredictorService;

        public GameController(IGamePredictorService gamePredictorService)
        {
            _gamePredictorService = gamePredictorService;
        }

        [Route("api/[controller]/{year}/{week}")]
        [HttpGet]
        public async Task<IEnumerable<Game>> Get(int year, int week)
        {
          return await _gamePredictorService.GetPredictions(year, week);
        }
    }
}
