using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TicTacToe.DataBaseModels;
using TicTacToe.Models;

namespace TicTacToe.Interfaces.Implementation
{
  public class ScoreboardService : DefaultService,IScoreboardService
  {
    public List<ScoreBoardModel> GetScoreBoard()
    {
      return Mapper.Map<List<Scoreboard>,List<ScoreBoardModel>>(DatabaseContext.Scoreboard.Include(x=>x.User).ToList());
    }

    public Result CreateScoreboardEntry(int userId)
    {
      DatabaseContext.Scoreboard.Add(new Scoreboard()
      {
        Drafts = 0,
        Loses = 0,
        Wins = 0,
        UserId = userId
      });
      DatabaseContext.SaveChanges();
      return Result.Success();
    }

    public Result UpdateLoses(int userId)
    {
      var userScoreBoard = DatabaseContext.Scoreboard.SingleOrDefault(x => x.UserId == userId);
      if (userScoreBoard != null)
      {
        var loses = userScoreBoard.Loses;
        userScoreBoard.Loses = loses + 1;
        DatabaseContext.SaveChanges();
        return Result.Success();
      }

      return Result.Failure("User doesn't exist");
    }

    public Result UpdateDrafts(int userId)
    {
      var userScoreBoard = DatabaseContext.Scoreboard.SingleOrDefault(x => x.UserId == userId);
      if (userScoreBoard != null)
      {
        var drafts = userScoreBoard.Drafts;
        userScoreBoard.Drafts = drafts + 1;
        DatabaseContext.SaveChanges();
        return Result.Success();
      }

      return Result.Failure("User doesn't exist");
    }

    public Result UpdateWins(int userId)
    {
      var userScoreBoard = DatabaseContext.Scoreboard.SingleOrDefault(x => x.UserId == userId);
      if (userScoreBoard != null)
      {
        var wins = userScoreBoard.Wins;
        userScoreBoard.Wins = wins + 1;
        DatabaseContext.SaveChanges();
        return Result.Success();
      }

      return Result.Failure("User doesn't exist");
    }
  }
}
