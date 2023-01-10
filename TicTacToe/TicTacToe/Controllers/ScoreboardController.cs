using Microsoft.AspNetCore.Mvc;
using TicTacToe.Interfaces;

namespace TicTacToe.Controllers
{
  [ApiController]
  [Route("api/scoreboard")]
  public class ScoreboardController : ControllerBase
  {
    private IScoreboardService _scoreboardService;

    public ScoreboardController(IScoreboardService scoreboardService)
    {
      _scoreboardService = scoreboardService;
    }


    [HttpGet]
    public IActionResult GetScoreboard()
    {
      return Ok(_scoreboardService.GetScoreBoard());
    } 
  }
}
