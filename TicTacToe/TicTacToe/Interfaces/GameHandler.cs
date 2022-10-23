using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
  public class GameHandler
  {
    public static List<GameModel> GameModels = new List<GameModel>();

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

    private static void MakeMove(char?[,] gameBoard, Tuple<int, int> position, char mark)
    {
      gameBoard[position.Item1, position.Item2] = mark;
    }

    private static GameResult CheckForEndGame(char?[,] gameBoard)
    {
      var result = CheckForWin(gameBoard);
      if (result != GameResult.InProgress)
        return result;
      return CheckForDraft(gameBoard);
    }

    private static GameResult CheckForDraft(char?[,] gameBoard)
    {
      for (var i = 0; i < 3; i++)
      for(var j = 0; j <3;j++)
          if (!gameBoard[i, j].HasValue)
            return GameResult.InProgress;

      return GameResult.Draft;
    }

    private static GameResult CheckForWin(char?[,] gameBoard)
    {
      var result = CheckForRowWin(gameBoard);
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      result = CheckForColumnWin(gameBoard);
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      result = CheckForDiagonalWin(gameBoard);
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      return GameResult.InProgress;
    }

    private static Result<char> CheckForRowWin(char?[,] gameBoard)
    {
      for (int i = 0; i < 2; i++)
      {
        if (!gameBoard[0, i].HasValue || !gameBoard[1, i].HasValue || !gameBoard[2, i].HasValue) continue;
        if (gameBoard[0, i].Value == gameBoard[1, i].Value && gameBoard[1, i].Value == gameBoard[2, i].Value)
          return Result.Success(gameBoard[0, i].Value);
      }
      return Result.Failure<char>("Game still in progress");
    }

    private static Result<char> CheckForColumnWin(char?[,] gameBoard)
    {
      for (int i = 0; i < 2; i++)
      {
        if (!gameBoard[i, 0].HasValue || !gameBoard[i, 1].HasValue || !gameBoard[i, 2].HasValue) continue;
        if (gameBoard[i, 0].Value == gameBoard[i, 1].Value && gameBoard[i, 1].Value == gameBoard[i, 2].Value)
          return Result.Success(gameBoard[i, 0].Value);
      }
      return Result.Failure<char>("Game still in progress");
    }

    private static Result<char> CheckForDiagonalWin(char?[,] gameBoard)
    {
      if (gameBoard[0, 0].HasValue && gameBoard[1, 1].HasValue && gameBoard[2, 2].HasValue)
        if (gameBoard[0, 0].Value == gameBoard[1, 1].Value && gameBoard[1, 1].Value == gameBoard[2, 2].Value)
          return Result.Success(gameBoard[0, 0].Value);
      if (gameBoard[0, 2].HasValue && gameBoard[1, 1].HasValue && gameBoard[2, 0].HasValue)
        if (gameBoard[0, 2].Value == gameBoard[1, 1].Value && gameBoard[1, 1].Value == gameBoard[2, 0].Value)
          return Result.Success(gameBoard[0, 2].Value);
      return Result.Failure<char>("Game still in progress");
    }

    public static Result<GameResult> MakeMove(MakeMoveModel makeMove)
    {
      var gameModel = FindGameByUserId(makeMove.userId);
      var mark = GetCharMark(gameModel, makeMove.userId);
      if (!IsLegalMove(gameModel.GameBoard, new Tuple<int, int>(makeMove.xPos,makeMove.yPos)))
        return Result.Failure<GameResult>("Position is taken");
      
      MakeMove(gameModel.GameBoard, new Tuple<int, int>(makeMove.xPos, makeMove.yPos), mark);
      return Result.Success<GameResult>(CheckForEndGame(gameModel.GameBoard));
    }
    


    public static PlayerInfo FindOpponent(int userId)
    {
      var gameModel = FindGameByUserId(userId);
      return gameModel.Player1.UserId == userId ? gameModel.Player2 : gameModel.Player1;
    }

    public static void Remove(GameModel gameModel)
    {
      GameModels.Remove(gameModel);
    }
  }
}
