using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.WebSockets
{
  public class WebSocketsHub : Hub<IGameClient>
  {
    public static Dictionary<int, string> WaitList = new Dictionary<int, string>();
    private IScoreboardService _scoreboardService;

    WebSocketsHub(IScoreboardService scoreboardService)
    {
      this._scoreboardService = scoreboardService;
    }

    public async Task MakeAMove(MakeMoveModel makeMove)
    {
      var result = GameHandler.MakeMove(makeMove);
      await result
        .TapError(async (x) => await Clients.Client(Context.ConnectionId).ReceiveMessage(x))
        .Tap(async (x) =>
        {
          await GenerateOutput(x, makeMove.userId);
        });
    }

    private async Task GenerateOutput(GameResult x, int userId)
    {
      var gameModel = GameHandler.FindGameByUserId(userId);
      var opponent = GameHandler.FindOpponent(userId);
      switch (x)
      {
        case GameResult.Draft:
          await HandleDraftScenario(gameModel);
          break;
        case GameResult.InProgress:
          await HandleGameInProgressScenario(GameHandler.FindOpponent(userId), GameHandler.FindPlayer(userId),
            gameModel.GameBoard);
          break;
        case GameResult.WinPlayer1:
          await HandleWinScenario(gameModel.Player1, gameModel.Player2, gameModel);
          break;
        case GameResult.WinPlayer2:
          await HandleWinScenario(gameModel.Player2, gameModel.Player1, gameModel);
          break;
      }
    }

    public async Task AddToWaitList(int userId)
    {
      WaitList.TryAdd(userId,Context.ConnectionId);
      var result = TryCreateGame();
      await result
        .Tap(StartGame)
        .TapError(x => Clients.Client(Context.ConnectionId).ReceiveMessage(x));
    }

    private async Task StartGame(GameModel gameModel)
    {
      await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(gameModel.Player1.ConnectionId).MakeMove();
    }

    private async Task HandleWinScenario(PlayerInfo winner, PlayerInfo loser, GameModel gameModel)
    {
      await Clients.Client(winner.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(loser.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(winner.ConnectionId).Win();
      await Clients.Client(loser.ConnectionId).Lost();
      _scoreboardService.UpdateWins(winner.UserId);
      _scoreboardService.UpdateLoses(loser.UserId);
      GameHandler.Remove(gameModel);
    }
    private async Task HandleDraftScenario(GameModel gameModel)
    {
      await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
      await Clients.Client(gameModel.Player1.ConnectionId).Draft();
      await Clients.Client(gameModel.Player2.ConnectionId).Draft();
      _scoreboardService.UpdateDrafts(gameModel.Player1.UserId);
      _scoreboardService.UpdateDrafts(gameModel.Player2.UserId);
      GameHandler.Remove(gameModel);
    }

    private async Task HandleGameInProgressScenario(PlayerInfo playerWithMove, PlayerInfo playerWithoutMove, char?[,] gameBoard)
    {
      await Clients.Client(playerWithMove.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameBoard));
      await Clients.Client(playerWithoutMove.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameBoard));
      await Clients.Client(playerWithMove.ConnectionId).MakeMove();
    }
    private Result<GameModel> TryCreateGame()
    {
      if (WaitList.Count > 1)
      {
        var player1 = WaitList.First();
        WaitList.Remove(player1.Key);
        var player2 = WaitList.First();
        WaitList.Remove(player2.Key);

        var gameModel = GameHandler.AddGame(
          new PlayerInfo(player1.Key,player1.Value),
          new PlayerInfo(player2.Key,player2.Value));
        return Result.Success<GameModel>(gameModel);
      }
      return Result.Failure<GameModel>("Not enough players to start game");
    }


  }
}
