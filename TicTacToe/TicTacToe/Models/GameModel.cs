using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
  public class GameModel
  {
    public PlayerInfo Player1 { get; set; }
    public PlayerInfo Player2 { get; set; }
    public string GameGuid { get; set; }
    public char[,] GameBoard { get; set; }

    public GameModel(PlayerInfo player1, PlayerInfo player2)
    {
      Player1 = player1;
      Player2 = player2;
      GameBoard = new char[3, 3];
      GameGuid = Guid.NewGuid().ToString();
    }
  }
}
