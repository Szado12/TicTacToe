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
      var sd = new int[3, 3];
      await result
        .TapError(async (x) => await Clients.Client(Context.ConnectionId).ReceiveMessage(x))
        .Tap(async (x) =>
        {
          if (x == GameResult.InProgress)
          {
            var z = GameHandler.FindGameByUserId(makeMove.userId);
            var y = GameHandler.FindOpponent(makeMove.userId);
            await Clients.Client(y.ConnectionId).MakeMove(JsonConvert.SerializeObject(sd));
          }
        });
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
      var z = new char?[3,3];
      //await Clients.Client(gameModel.Player1.ConnectionId).StartGame(gameModel.Player2.UserId.ToString());
      //await Clients.Client(gameModel.Player2.ConnectionId).StartGame(gameModel.Player1.UserId.ToString());
      await Clients.Client(gameModel.Player1.ConnectionId).MakeMove(JsonConvert.SerializeObject(gameModel.GameBoard));
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
