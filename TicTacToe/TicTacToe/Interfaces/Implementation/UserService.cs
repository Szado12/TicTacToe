using System;
using System.Linq;
using CSharpFunctionalExtensions;
using TicTacToe.DataBaseModels;
using TicTacToe.Models.Users;

namespace TicTacToe.Interfaces.Implementation
{
  public class UserService : DefaultService, IUserService
  {

    private IScoreboardService _scoreboardService;
    public UserService(IScoreboardService scoreboardService)
    {
      _scoreboardService = scoreboardService;
    }

    public Result<UserModelLoginReturn> Register(string username, string password)
    {
      if (DatabaseContext.Users.SingleOrDefault(x => x.Username == username) != null)
      {
        return Result.Failure<UserModelLoginReturn>("User with same username exist");
      }

      var user = new Users()
      {
        Username = username,
        Password = password
      };

      DatabaseContext.Users.Add(user);
      if (DatabaseContext.SaveChanges() <= 0)
        return Result.Failure<UserModelLoginReturn>("An Error happened while registering user");

      _scoreboardService.CreateScoreboardEntry(user.UserId);

      return Login(username, password);
    }

    public Result<UserModelLoginReturn> Login(string username, string password)
    {
      var user = DatabaseContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password).AsMaybe();
      return user.HasValue 
        ? Result.Success(Mapper.Map<Users, UserModelLoginReturn>(user.Value)) 
        : Result.Failure<UserModelLoginReturn>("There is no user with provided credentials");
    }
  }
}
