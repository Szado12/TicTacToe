using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
  public class PlayerInfo
  {
    public int UserId { get; set; }
    public string ConnectionId { get; set; }

    public PlayerInfo(int userId, string connectionId)
    {
      UserId = userId;
      ConnectionId = connectionId;
    }
  }
}
