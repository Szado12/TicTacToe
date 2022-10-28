using System.Collections.Generic;
using CSharpFunctionalExtensions;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
  interface IScoreboardService
  {
    List<ScoreBoardModel> GetScoreBoard();
    Result UpdateLoses(int userId);
    Result UpdateDrafts(int userId);
    Result UpdateWins(int userId);
  }
}
