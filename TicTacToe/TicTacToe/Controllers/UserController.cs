using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using TicTacToe.DataBaseModels;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [Route("Login")]
    [HttpPost]
    public IActionResult Login(UserModel userLoginModel)
    {
      var userResult = _userService.Login(userLoginModel.Username, userLoginModel.Password);
      return userResult.IsSuccess 
        ? (IActionResult) Ok(userResult.Value) 
        : Unauthorized();
    }

    [HttpPost]
    [Route("Register")]
    public IActionResult Register(UserModel userRegisterModel)
    {
      return _userService.Register(userRegisterModel.Username, userRegisterModel.Password).IsSuccess 
        ? (IActionResult) Ok() 
        : BadRequest();
    }
  }
}
