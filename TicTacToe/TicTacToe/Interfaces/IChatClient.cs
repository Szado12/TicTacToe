using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
  public interface IChatClient
  {
    Task ReceiveMessage(MessageModel message);
  }
}
