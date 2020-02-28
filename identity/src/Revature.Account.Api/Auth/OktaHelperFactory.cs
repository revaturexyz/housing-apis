using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Revature.Account.Api
{
  /// <summary>
  /// Factory class for creating and OktaHelper. Main purpose
  /// is to promote testability.
  /// </summary>
  public class OktaHelperFactory : IOktaHelperFactory
  {
    private readonly ILoggerFactory _loggerFactory;

    public OktaHelperFactory(ILoggerFactory loggerFactory)
    {
      _loggerFactory = loggerFactory ?? throw new System.ArgumentNullException(nameof(loggerFactory));
    }

    public OktaHelper Create(HttpRequest request)
    {
      var logger = _loggerFactory.CreateLogger("Revature.Account.Api.OktaHelper");
      var okta = new OktaHelper(request, logger);
      okta.ConnectManagementClient();
      return okta;
    }
  }
}
