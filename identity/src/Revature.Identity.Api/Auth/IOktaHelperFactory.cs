using Microsoft.AspNetCore.Http;

namespace Revature.Identity.Api
{
  public interface IOktaHelperFactory
  {
    OktaHelper Create(HttpRequest request);
  }
}
