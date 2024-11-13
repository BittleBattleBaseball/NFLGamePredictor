using Microsoft.AspNetCore.Mvc;
using NFLGamePredictor.Contracts;

namespace NFLGamePredictor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamRankController : ControllerBase
    {
        private IGamePredictorService _gamePredictorService;

        public TeamRankController(IGamePredictorService gamePredictorService)
        {
            _gamePredictorService = gamePredictorService;
        }

        [Route("api/[controller]/{year}/{week}")]
        [HttpGet]
        public async Task<IEnumerable<Team>> Get(int year, int week)
        {
          return await _gamePredictorService.GetWeeklyTeamRankings(year, week);
        }
    }
}
