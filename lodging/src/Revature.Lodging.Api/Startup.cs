using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer; // OktaSetup
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens; // OktaSetup
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using Revature.Lodging.Api.Services;
using Revature.Lodging.Api.Telemetry;
using Revature.Lodging.DataAccess;
using Revature.Lodging.DataAccess.Entities;
using Revature.Lodging.DataAccess.Repository;
using Revature.Lodging.Lib.Interface;
using Serilog;

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

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(CorsPolicyName, builder =>
        {
          builder.WithOrigins(
            "http://localhost:4200",
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
            "https://localhost:13080",
            "https://housing-angular-dev.azurewebsites.net")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
      });

      services.AddAuthentication(options =>
      {
        // options.DefaultScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.Authority = Configuration["Okta:Domain"] + "/oauth2/default";
        options.Audience = "api://default";
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          NameClaimType = "name",
          RoleClaimType = "groups",
          ValidateIssuer = true,
        };
      });

      services.AddApplicationInsightsTelemetry();

      services.AddHealthChecks();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Revature Lodging", Version = "v1" });
      });

      services.AddDbContext<LodgingDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName)));

      services.AddScoped<IComplexRepository, ComplexRepository>();
      services.AddScoped<IAmenityRepository, AmenityRepository>();
      services.AddScoped<IRoomRepository, RoomRepository>();

      // services.AddHostedService<RoomServiceReceiver>();
      services.AddHttpClient<IAddressRequest, AddressRequest>();
      services.AddSingleton<ITelemetryInitializer, LodgingTelemetryInitializer>();

      services.AddAuthorization();

      services.AddControllers();
    }

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

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health");
      });
    }
  }
}
