using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using TicTacToe.DataBaseModels;
using TicTacToe.Models;
using TicTacToe.Models.Users;

namespace TicTacToe.MapperProfile
{
  public class MapperProfile : Profile
  {
    public MapperProfile()
    {
      CreateMap<Users, UserModelLoginReturn>();
      CreateMap<Scoreboard, ScoreBoardModel>().ForMember(s => s.UserName, opt => opt.MapFrom(src => src.User.Username));
    }
  }
}
