using Microsoft.AspNetCore.Http;

namespace Revature.Account.Api
{
  public interface IOktaHelperFactory
  {
    public OktaHelper Create(HttpRequest request);
  }
}
