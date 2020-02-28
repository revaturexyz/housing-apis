using Microsoft.AspNetCore.Http;

namespace Revature.Account.Api
{
  public interface IOktaHelperFactory
  {
    OktaHelper Create(HttpRequest request);
  }
}
