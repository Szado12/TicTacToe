using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using TicTacToe.DataBaseModels;
using TicTacToe.Interfaces;
using TicTacToe.Interfaces.Implementation;
using TicTacToe.WebSockets;

namespace TicTacToe
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    private readonly string swaggerBasePath = "api";
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddSignalR();
      services.AddTransient<IScoreboardService, ScoreboardService>();
      services.AddTransient<IUserService, UserService>(userService => new UserService(userService.GetRequiredService<IScoreboardService>()));

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "Tic Tac Toe",
          Description = "Tic Tac Toe",
        });

      });

      services.AddCors(options =>
      {
        options.AddPolicy("ClientPermission", policy =>
        {
          policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
        });
      });
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSwagger(c =>
      {
        c.RouteTemplate = swaggerBasePath + "/{documentName}/swagger.json";
      });

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint($"/{swaggerBasePath}/v1/swagger.json", $"APP API");
        c.RoutePrefix = $"{swaggerBasePath}";
      });

      app.UseHttpsRedirection();
      app.UseCors("ClientPermission");
      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<WebSocketsHub>("/api/game");
      });
    }
  }
}
