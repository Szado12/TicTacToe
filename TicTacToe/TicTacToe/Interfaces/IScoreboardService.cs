using System.Collections.Generic;
using CSharpFunctionalExtensions;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
  public interface IScoreboardService
  {
    List<ScoreBoardModel> GetScoreBoard();
    public Result CreateScoreboardEntry(int userId);
    Result UpdateLoses(int userId);
    Result UpdateDrafts(int userId);
    Result UpdateWins(int userId);
  }
}
