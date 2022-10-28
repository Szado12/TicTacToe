using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TicTacToe.Extensions;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
  public class GameHandler
  {
    private static readonly List<GameModel> GameModels = new List<GameModel>();

    public static GameModel AddGame(PlayerInfo player1, PlayerInfo player2)
    {
      var gameModel = new GameModel(player1, player2);
      GameModels.Add(gameModel);
      return gameModel;
    }

    public static GameModel FindGameByUserId(int userId)
    {
      return GameModels.FirstOrDefault(x => x.Player1.UserId == userId || x.Player2.UserId == userId);
    }

    private static char GetCharMark(GameModel gameModel, int userId)
    {
      return gameModel.Player1.UserId == userId ? 'o' : 'x';
    }

    private static bool IsLegalMove(char?[,] gameBoard, Tuple<int, int> position)
    {
      return !gameBoard[position.Item1, position.Item2].HasValue;
    }

    public static Result<GameResult> MakeMove(MakeMoveModel makeMove)
    {
      var gameModel = FindGameByUserId(makeMove.userId);
      var mark = GetCharMark(gameModel, makeMove.userId);
      if (!IsLegalMove(gameModel.GameBoard, new Tuple<int, int>(makeMove.xPos,makeMove.yPos)))
        return Result.Failure<GameResult>("Position is taken");
      
      gameModel.GameBoard.MakeMove(new Tuple<int, int>(makeMove.xPos, makeMove.yPos), mark);
      return Result.Success<GameResult>(gameModel.GameBoard.CheckForEndGame());
    }
    
    public static PlayerInfo FindOpponent(int userId)
    {
      var gameModel = FindGameByUserId(userId);
      return gameModel.Player1.UserId == userId ? gameModel.Player2 : gameModel.Player1;
    }

    public static PlayerInfo FindPlayer(int userId)
    {
      var gameModel = FindGameByUserId(userId);
      return gameModel.Player1.UserId == userId ? gameModel.Player1 : gameModel.Player2;
    }

    public static void Remove(GameModel gameModel)
    {
      GameModels.Remove(gameModel);
    }
  }
}
