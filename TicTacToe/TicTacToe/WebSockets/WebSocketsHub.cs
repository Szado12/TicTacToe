using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.WebSockets
{
  public class WebSocketsHub : Hub<IChatClient>
  {
    public static Dictionary<int, string> WaitList = new Dictionary<int, string>();

    public async Task SendMessage(MessageModel messageModel)
    {
      await Clients.All.ReceiveMessage(messageModel);
    }

    public bool MakeAMove(int userId, Tuple<int,int> position)
    {
      return true;
    }

    public async Task AddToWaitList(int userId)
    {
      WaitList.TryAdd(userId,Context.ConnectionId);
      var result = TryCreateGame();
      await result
        .Tap(x => StartGame(x))
        .TapError(_ => SendMessage(new MessageModel()));
    }

    private async Task StartGame(GameModel gameModel)
    {
      await Clients.Client(gameModel.Player1.ConnectionId).StartGame(new MessageModel());
      await Clients.Client(gameModel.Player2.ConnectionId).StartGame(new MessageModel());
      await Clients.Client(gameModel.Player1.ConnectionId).MakeMove(new MessageModel());
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
