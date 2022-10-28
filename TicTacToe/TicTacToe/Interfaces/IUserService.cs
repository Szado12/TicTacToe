using CSharpFunctionalExtensions;
using TicTacToe.Models.Users;

namespace TicTacToe.Interfaces
{
  public interface IUserService
  {
    Result Register(string username, string password);
    Result<UserModelLoginReturn> Login(string username, string password);
  }
}
