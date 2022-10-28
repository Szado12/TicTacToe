using AutoMapper;
using TicTacToe.DataBaseModels;

namespace TicTacToe.Interfaces.Implementation
{
  public class DefaultService
  {
    public readonly DataBaseContext DatabaseContext = new DataBaseContext();
    private static readonly MapperConfiguration MapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile.MapperProfile()));
    protected IMapper Mapper = MapperConfig.CreateMapper();
  }
}
