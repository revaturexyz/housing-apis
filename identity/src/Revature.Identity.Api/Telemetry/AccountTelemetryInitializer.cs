using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Revature.Identity.Api.Telemetry
{
  internal class AccountTelemetryInitializer : ITelemetryInitializer
  {
    private readonly IHttpContextAccessor _contextAccessor;

    public AccountTelemetryInitializer(IHttpContextAccessor ctxAccessor)
    {
      _contextAccessor = ctxAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
      var ctx = _contextAccessor.HttpContext;

      if (ctx != null)
      {
        var requestTelemetry = ctx.Features.Get<RequestTelemetry>();

        if (requestTelemetry != null && !string.IsNullOrEmpty(requestTelemetry.Context.User.Id) &&
          (string.IsNullOrEmpty(telemetry.Context.User.Id) || string.IsNullOrEmpty(telemetry.Context.Session.Id)))
        {
          // Set the user id on the Application Insights telemetry item.
          telemetry.Context.User.Id = requestTelemetry.Context.User.Id;

          // Set the session id on the Application Insights telemetry item.
          telemetry.Context.Session.Id = requestTelemetry.Context.User.Id;
        }
      }
    }
  }
}
