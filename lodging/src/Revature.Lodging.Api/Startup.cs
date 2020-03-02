using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using Revature.Lodging.Api.Services;
using Revature.Lodging.DataAccess;
using Revature.Lodging.DataAccess.Entities;
using Revature.Lodging.DataAccess.Repository;
using Revature.Lodging.Lib.Interface;
using Serilog;
using Microsoft.ApplicationInsights.Extensibility;
using Revature.Lodging.Api.Telemetry;

namespace Revature.Lodging.Api
{
  public class Startup
  {
    private const string ConnectionStringName = "LodgingDb";
    private const string CorsPolicyName = "RevatureCorsPolicy";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    /// <summary>
    /// to configure the services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(CorsPolicyName, builder =>
        {
          builder.WithOrigins("http://localhost:4200",
                              "https://localhost:4200",
                              "http://housing.revature.xyz",
                              "https://housing.revature.xyz",
                              "http://housingdev.revature.xyz",
                              "https://housingdev.revature.xyz",
                              "http://192.168.99.100:12080",
                              "https://192.168.99.100:12080",
                              "http://192.168.99.100:13080",
                              "https://192.168.99.100:13080",
                              "http://192.168.99.100:14080",
                              "https://192.168.99.100:14080",
                              "http://localhost:14080",
                              "https://localhost:14080",
                              "http://localhost:13080",
                              "https://localhost:13080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
      });

      services.AddApplicationInsightsTelemetry();
      
      services.AddHealthChecks();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Revature Lodging", Version = "v1" });
      });

      services.AddDbContext<LodgingDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName)));

      services.AddScoped<IRepository, Repository>();
      services.AddScoped<IMapper, Mapper>();
      // services.AddHostedService<RoomServiceReceiver>();
      // services.AddScoped<IRoomServiceSender, RoomServiceSender>();
      services.AddHttpClient<IAddressRequest, AddressRequest>();
      services.AddHttpClient<IRoomRequest, RoomRequest>();
      services.AddSingleton<ITelemetryInitializer, LodgingTelemetryInitializer>();

      //services.AddAuthentication(options => 
      //    {
      //    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
      //    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
      //    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
      //  })
      //  .AddOktaWebApi(new OktaWebApiOptions()
      //  {
      //    OktaDomain = Configuration["Okta:OktaDomain"],
      //  }

      //);

      //services.AddAuthorization();

      services.AddControllers();

    }

    /// <summary>
    /// it is to create the app's request processing pipeline
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSerilogRequestLogging();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revature Lodging V1");
      });

      app.UseRouting();

      app.UseCors(CorsPolicyName);

      //app.UseAuthentication();

      //app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health");
      });

      ////for the service-bus listener
      ////define the event-listener
      //var bus = app.ApplicationServices.GetService<IRoomServiceReceiver>();

      ////start listening
      //bus.RegisterOnMessageHandlerAndReceiveMessages();
    }
  }
}
