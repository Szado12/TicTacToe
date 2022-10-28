using System;
using CSharpFunctionalExtensions;
using TicTacToe.Models;

namespace TicTacToe.Extensions
{
  public static class CharArrayExtension
  {
    private static Result<char> CheckForColumnWin(this char?[,] gameBoard)
    {
      for (int i = 0; i < 2; i++)
      {
        if (!gameBoard[i, 0].HasValue || !gameBoard[i, 1].HasValue || !gameBoard[i, 2].HasValue) continue;
        if (gameBoard[i, 0].Value == gameBoard[i, 1].Value && gameBoard[i, 1].Value == gameBoard[i, 2].Value)
          return Result.Success(gameBoard[i, 0].Value);
      }
      return Result.Failure<char>("Game still in progress");
    }

    public static GameResult CheckForEndGame(this char?[,] gameBoard)
    {
      var result = gameBoard.CheckForWin();
      if (result != GameResult.InProgress)
        return result;
      return gameBoard.CheckForDraft();
    }
    private static Result<char> CheckForDiagonalWin(this char?[,] gameBoard)
    {
      if (gameBoard[0, 0].HasValue && gameBoard[1, 1].HasValue && gameBoard[2, 2].HasValue)
        if (gameBoard[0, 0].Value == gameBoard[1, 1].Value && gameBoard[1, 1].Value == gameBoard[2, 2].Value)
          return Result.Success(gameBoard[0, 0].Value);
      if (gameBoard[0, 2].HasValue && gameBoard[1, 1].HasValue && gameBoard[2, 0].HasValue)
        if (gameBoard[0, 2].Value == gameBoard[1, 1].Value && gameBoard[1, 1].Value == gameBoard[2, 0].Value)
          return Result.Success(gameBoard[0, 2].Value);
      return Result.Failure<char>("Game still in progress");
    }

    private static GameResult CheckForDraft(this char?[,] gameBoard)
    {
      for (var i = 0; i < 3; i++)
      for (var j = 0; j < 3; j++)
        if (!gameBoard[i, j].HasValue)
          return GameResult.InProgress;

      return GameResult.Draft;
    }
    private static Result<char> CheckForRowWin(this char?[,] gameBoard)
    {
      for (int i = 0; i < 2; i++)
      {
        if (!gameBoard[0, i].HasValue || !gameBoard[1, i].HasValue || !gameBoard[2, i].HasValue) continue;
        if (gameBoard[0, i].Value == gameBoard[1, i].Value && gameBoard[1, i].Value == gameBoard[2, i].Value)
          return Result.Success(gameBoard[0, i].Value);
      }
      return Result.Failure<char>("Game still in progress");
    }

    private static GameResult CheckForWin(this char?[,] gameBoard)
    {
      var result = gameBoard.CheckForRowWin();
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      result = gameBoard.CheckForColumnWin();
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      result = gameBoard.CheckForDiagonalWin();
      if (result.IsSuccess)
        return result.Value == 'o' ? GameResult.WinPlayer1 : GameResult.WinPlayer2;
      return GameResult.InProgress;
    }

    public static void MakeMove(this char?[,] gameBoard, Tuple<int, int> position, char mark)
    {
      gameBoard[position.Item1, position.Item2] = mark;
    }
  }
}
