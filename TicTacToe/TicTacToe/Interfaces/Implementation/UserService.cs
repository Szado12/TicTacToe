using System;
using System.Linq;
using CSharpFunctionalExtensions;
using TicTacToe.DataBaseModels;
using TicTacToe.Models.Users;

namespace TicTacToe.Interfaces.Implementation
{
  public class UserService : DefaultService, IUserService
  {
    public Result Register(string username, string password)
    {
      if (DatabaseContext.Users.SingleOrDefault(x => x.Username == username) != null)
      {
        return Result.Failure("User with same username exist");
      }

      DatabaseContext.Users.Add(new Users()
      {
        Username = username,
        Password = password
      });
      return DatabaseContext.SaveChanges() > 0
        ? Result.Success()
        : Result.Failure("An Error happened while user was registered");
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
