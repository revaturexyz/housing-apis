using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace Revature.Identity.Api
{
  public class OktaHelper
  {
    public static readonly string CoordinatorRole = "00g2qoe8fD5otteWj4x6";
    public static readonly string ApprovedProviderRole = "00g2sl5lo8uAc4h7I4x6";
    public static readonly string TenantRole = "00g2skdaoGOwpGSHj4x6";

    private readonly ILogger _logger;

    public OktaHelper(Microsoft.AspNetCore.Http.HttpRequest request, ILogger logger)
    {
      string jwt = request.Headers["Authorization"];

      // Remove 'Bearer '
      jwt = jwt.Replace("Bearer ", string.Empty);
      var handler = new JwtSecurityTokenHandler();
      var token = handler.ReadJwtToken(jwt) as JwtSecurityToken;

      Email = (string)token.Payload["sub"];
      Roles = JsonSerializer.Deserialize<string[]>(token.Payload["groups"].ToString());
      _logger = logger;
    }

    public static string Token { get; private set; }

    public static string Domain { get; private set; }

    public static string ClientId { get; private set; }

    public static string Secret { get; private set; }

    public OktaClient Client { get; private set; }

    public string Email { get; private set; }

    public IEnumerable<string> Roles { get; private set; }

    public JsonElement AppMetadata { get; private set; }

    /// <summary>
    /// Function to set the secret values, intended for use in Startup.
    /// </summary>
    public static void SetSecretValues(string domain, string clientId, string secret, string token)
    {
      Domain = domain;
      ClientId = clientId;
      Secret = secret;
      Token = token;
    }

    /// <summary>
    /// Runs code to set up the management client, which involves sending a request to Okta in order to get
    /// an authenticated token. Moved to a function so that it can be ignored if we just want
    /// to read the token's values.
    /// </summary>
    public bool ConnectManagementClient()
    {
      try
      {
        Client = new OktaClient(new OktaClientConfiguration
        {
          OktaDomain = Domain,
          Token = Token
        });
      }
      catch (JsonException ex)
      {
        _logger.LogError(ex, "Error while processing Okta response");
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogError(ex, "Error while processing Okta response");
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogError(ex, "Error while processing Okta response");
      }

      return false;
    }

    /// <summary>
    /// Adds a role to the remote Okta profile.
    /// </summary>
    /// <param name="oktaUserId">UserId according to Okta. Has to be retrieved from the Management client.</param>
    /// <param name="roleId">RoleId according to Okta. Has to be retrieved from the Management client.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddRoleAsync(string oktaUserId, string roleId)
    {
      await Client.Groups.AddUserToGroupAsync(roleId, oktaUserId);
    }

    /// <summary>
    /// Removes a role from the remote Okta profile.
    /// </summary>
    /// <param name="oktaUserId">UserId according to Okta. Has to be retrieved from the Management client.</param>
    /// <param name="roleId">RoleId according to Okta. Has to be retrieved from the Management client.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RemoveRoleAsync(string oktaUserId, string roleId)
    {
      await Client.Groups.RemoveGroupUserAsync(roleId, oktaUserId);
    }

    /// <summary>
    /// Updates remote Okta profile's app metadata to include the given Revature account id.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task UpdateMetadataWithIdAsync(string oktaUserId, Guid newId)
    {
      var user = await Client.Users.GetUserAsync(oktaUserId);
      await Client.Users.UpdateUserAsync(user, oktaUserId, null);
    }
  }
}
