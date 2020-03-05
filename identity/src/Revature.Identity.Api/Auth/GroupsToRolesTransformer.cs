using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace Revature.Identity.Api.Auth
{
public class GroupsToRolesTransformer : IClaimsTransformation
  {
    private OktaClient client;

    public GroupsToRolesTransformer(IConfiguration configuration)
    {
      Configuration = configuration;

      client = new OktaClient(new OktaClientConfiguration{
            OktaDomain = Configuration["Okta:Domain"],
            Token = Configuration["Okta:ApiToken"],
        });
    }

    public IConfiguration Configuration { get; }

    /*
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsTransformationContext context)
    {
      var idClaim = context.Principal.FindFirst(x=>x.Type == ClaimTypes.NameIdentifier);
      if(idClaim != null)
      {
          var user = await client.Users.GetUserAsync(idClaim.Value);
          if(user != null){
            var groups = user.Groups.ToEnumerable();
            foreach (var group in groups)
            {
                ((ClaimsIdentity)context.Principal.Identity).AddClaim(new Claim(ClaimTypes.Role, group.Profile.Name));
            }
          }
      }
      return context.Principal;
    }
    */

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
      throw new System.NotImplementedException();
    }
  }
}
