using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Revature.Lodging.DataAccess.Entities;
using Serilog;
using Serilog.Events;

namespace Revature.Lodging.Api
{
  public static class Program
  {
    public static async Task Main(string[] args)
    {
      ConfigureLogger();

      try
      {
        Log.Information("Building web host");
        using var host = CreateHostBuilder(args).Build();
        await EnsureDatabaseCreatedAsync(host);

        Log.Information("Starting web host");
        await host.RunAsync();
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
      }
#pragma warning restore CA1031 // Do not catch general exception types
      finally
      {
        Log.CloseAndFlush();
      }
    }

    /// <summary>
    /// Configure Serilog logger globally.
    /// </summary>
    public static void ConfigureLogger()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();
    }

    /// <summary>
    /// Create host builder.
    /// </summary>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
          webBuilder.UseSerilog();
        });
    }

    /// <summary>
    /// Ensure the lodging database is created.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task EnsureDatabaseCreatedAsync(IHost host)
    {
      using var scope = host.Services.CreateScope();
      var serviceProvider = scope.ServiceProvider;

      Log.Information("Ensuring database created");
      using var context = serviceProvider.GetRequiredService<LodgingDbContext>();

      var created = await context.Database.EnsureCreatedAsync();
      if (created)
      {
        Log.Information("Database created");
      }
      else
      {
        Log.Information("Database already exists; not created");
      }
    }
  }
}
