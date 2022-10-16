using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.WebSockets
{
  public class WebSocketsHub : Hub<IChatClient>
  {
    public async Task SendMessage(MessageModel messageModel)
    {
      await Clients.All.ReceiveMessage(messageModel);
    }
  }
}
