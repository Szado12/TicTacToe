using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    private static GameModel FindGameByUserId(int userId)
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
      return GameResult.Draft;
    }

    private static GameResult CheckForWin(char?[,] gameBoard)
    {
      return GameResult.InProgress;
    }

    public static GameResult MakeMove(int userId,Tuple<int,int> position)
    {
      var gameModel = FindGameByUserId(userId);
      var mark = GetCharMark(gameModel, userId);
      if (IsLegalMove(gameModel.GameBoard, position))
        MakeMove(gameModel.GameBoard, position,mark);

      return CheckForEndGame(gameModel.GameBoard);
    }

    public static PlayerInfo FindOpponent(int userId)
    {
      var gameModel = FindGameByUserId(userId);
      return gameModel.Player1.UserId == userId ? gameModel.Player2 : gameModel.Player1;
    }
  }
}
