using System.Threading.Tasks;

namespace TicTacToe.Interfaces
{
  public interface IGameClient
  {
    Task ReceiveMessage(string message);
    Task AddedToWaitList();
    Task MakeMove();
    Task UpdateBoard(string board);
    Task Draft();
    Task Win();
    Task Lost();
  }
}
