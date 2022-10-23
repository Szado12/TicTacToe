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
  public class WebSocketsHub : Hub<IChatClient>
  {
    public static Dictionary<int, string> WaitList = new Dictionary<int, string>();

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
      var y = GameHandler.FindOpponent(userId);
      switch (x)
      {
        case GameResult.Draft:
          await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player1.ConnectionId).Draft();
          await Clients.Client(gameModel.Player2.ConnectionId).Draft();
          GameHandler.Remove(gameModel);
          break;
        case GameResult.InProgress:
          await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(y.ConnectionId).MakeMove("");
          break;
        case GameResult.WinPlayer1:
          await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player1.ConnectionId).Win();
          await Clients.Client(gameModel.Player2.ConnectionId).Lost();
          GameHandler.Remove(gameModel);
          break;
        case GameResult.WinPlayer2:
          await Clients.Client(gameModel.Player1.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player2.ConnectionId).UpdateBoard(JsonConvert.SerializeObject(gameModel.GameBoard));
          await Clients.Client(gameModel.Player1.ConnectionId).Lost();
          await Clients.Client(gameModel.Player2.ConnectionId).Win();
          GameHandler.Remove(gameModel);
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
      await Clients.Client(gameModel.Player1.ConnectionId).MakeMove("");
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
