using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Revature.Identity.DataAccess;
using Revature.Identity.Lib.Interface;
using Revature.Identity.DataAccess.Repositories;
using Revature.Identity.Api.Auth;
using Revature.Identity.Api.Filters;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using Serilog;

namespace Identity
{
  public class Startup
  {
  private const string ConnectionStringName = "IdentityDb";
  private const string CorsPolicyName = "RevatureCorsPolicy";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddDbContext<IdentityDbContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString(ConnectionStringName)));
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
                              "https://housing-angular-dev.azurewebsites.net")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
      });
      services.AddSingleton<IMapper, Mapper>();
      services.AddScoped<IGenericRepository, GenericRepository>();
      services.AddTransient<IOktaHelperFactory, OktaHelperFactory>();
      services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Revature Account", Version = "v1" });
        c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.ApiKey,
          Description = "Bearer authentication scheme with JWT, e.g. \"Bearer eyJhbGciOiJIUzI1NiJ9.e30\"",
          Name = "Authorization",
          In = ParameterLocation.Header
        });
        c.OperationFilter<SwaggerFilter>();
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
      })
          .AddOktaWebApi(new OktaWebApiOptions()
          {
            OktaDomain = Configuration["Okta:OktaDomain"],
          });

      services.AddAuthorization();
      services.AddHealthChecks();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revature Account V1");
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

      // Found at https://stackoverflow.com/questions/36958318/where-should-i-put-database-ensurecreated
      var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
      var serviceScope = serviceScopeFactory.CreateScope();
      var dbContext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();
      dbContext.Database.EnsureCreated();
    }
  }
}
