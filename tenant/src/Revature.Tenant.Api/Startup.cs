using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using Revature.Tenant.Api.ServiceBus;
using Revature.Tenant.Api.Telemetry;
using Revature.Tenant.DataAccess;
using Revature.Tenant.DataAccess.Entities;
using Revature.Tenant.DataAccess.Repository;
using Revature.Tenant.Lib.Interface;
using Serilog;

namespace Revature.Tenant.Api
{
  public class Startup
  {
    private const string ConnectionStringName = "TenantDb";
    private const string CorsPolicyName = "RevatureCorsPolicy";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddApplicationInsightsTelemetry();

      services.AddHealthChecks();

      services.AddDbContext<TenantContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName)));

      services.AddCors(options =>
      {
        options.AddPolicy(CorsPolicyName, builder =>
        {
          builder.WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200",
            "http://192.168.99.100:15080",
            "http://housing.revature.xyz",
            "https://housing.revature.xyz",
            "http://housingdev.revature.xyz",
            "https://housingdev.revature.xyz",
            "https://housing-angular-dev.azurewebsites.net")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Revature Tenant", Version = "v1" });
      });

      services.AddHttpClient<IRoomService, RoomService>();

      services.AddScoped<ITenantRepository, TenantRepository>();
      services.AddScoped<ITenantRoomRepository, TenantRoomRepository>();
      services.AddScoped<IMapper, Mapper>();
      services.AddScoped<IServiceBusSender, ServiceBusSender>();
      services.AddScoped<IIdentityService, IdentityService>();
      services.AddSingleton<ITelemetryInitializer, TenantTelemetryInitializer>();

      services.AddHttpClient<IAddressService, AddressService>();

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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revature Tenant V1");
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
