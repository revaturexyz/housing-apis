using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Okta.AspNetCore;
using Microsoft.Extensions.Logging;
using RestSharp;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace Revature.Account.Api
{
  public class OktaHelper
  {
    public static readonly string CoordinatorRole = "coordinator";
    public static readonly string UnapprovedProviderRole = "unapproved_provider";
    public static readonly string ApprovedProviderRole = "approved_provider";

    private readonly ILogger _logger;

    public OktaClient Client { get; private set; }
    public string Email { get; private set; }
    public IEnumerable<string> Roles { get; private set; }
    public JsonElement AppMetadata { get; private set; }

    public static string Domain { get; private set; }

    public static string Audience { get; private set; }

    public static string ClientId { get; private set; }

    public static string Secret { get; private set; }

    public static string ClaimsDomain { get; } = "https://dev-808810.okta.com/";

    /// <summary>
    /// Function to set the secret values, intended for use in Startup.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="audience"></param>
    /// <param name="clientId"></param>
    /// <param name="secret"></param>
    public static void SetSecretValues(string domain, string audience, string clientId, string secret)
    {
      Domain = domain;
      Audience = audience;
      ClientId = clientId;
      Secret = secret;
    }

    public OktaHelper(Microsoft.AspNetCore.Http.HttpRequest request, ILogger logger)
    {
      string jwt = request.Headers["Authorization"];
      // Remove 'Bearer '
      jwt = jwt.Replace("Bearer", "");
      var handler = new JwtSecurityTokenHandler();
      var token = handler.ReadJwtToken(jwt);

      Email = (string)token.Payload[ClaimsDomain + "email"];
      Roles = JsonSerializer.Deserialize<string[]>(token.Payload[ClaimsDomain + "roles"].ToString());
      // Will only need the id field from the app metadata
      AppMetadata = JsonSerializer.Deserialize<dynamic>(token.Payload[ClaimsDomain + "app_metadata"].ToString());
      _logger = logger;
    }

    /// <summary>
    /// Runs code to set up the management client, which involves sending a request to Auth0 in order to get
    /// an authenticated token. Moved to a function so that it can be ignored if we just want
    /// to read the token's values.
    /// </summary>
    /// <returns></returns>
    public bool ConnectManagementClient()
    {
      var client = new RestClient($"{Domain}");
      var request = new RestRequest(Method.POST);

      request.AddHeader("content-type", "application/json");
      request.AddParameter("application/json", $"{{\"client_id\":\"{ClientId}\",\"client_secret\":\"{Secret}\",\"audience\":\"https://{Domain}/api/v2/\",\"grant_type\":\"client_credentials\"}}", ParameterType.RequestBody);

      var response = client.Execute(request);

      if (response.ErrorException != null)
      {
        _logger.LogError(response.ErrorException, "Error while making Okta request");
        return false;
      }
      try
      {
        var deserializedResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var managementToken = deserializedResponse.GetProperty("access_token").GetString();
        Client = new OktaClient(new OktaClientConfiguration 
        {
          OktaDomain = Domain,
          Token = managementToken
        });
      }
      catch (JsonException ex)
      {
        _logger.LogError(ex, "Error while processing Auth0 response");
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogError(ex, "Error while processing Auth0 response");
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogError(ex, "Error while processing Auth0 response");
      }
      return false;
    }

    /// <summary>
    /// Adds a role to the remote Auth0 profile.
    /// </summary>
    /// <param name="oktaUserId">UserId according to Auth0. Has to be retrieved from the Management client.</param>
    /// <param name="roleId">RoleId according to Auth0. Has to be retrieved from the Management client.</param>
    /// <returns></returns>
    public async Task AddRoleAsync(string oktaUserId, string roleId)
    {
      await Client.Groups.AddUserToGroupAsync(roleId, oktaUserId);
    }

    /// <summary>
    /// Removes a role from the remote Auth0 profile.
    /// </summary>
    /// <param name="oktaUserId">UserId according to Auth0. Has to be retrieved from the Management client.</param>
    /// <param name="roleId">RoleId according to Auth0. Has to be retrieved from the Management client.</param>
    /// <returns></returns>
    public async Task RemoveRoleAsync(string oktaUserId, string roleId)
    {
      await Client.Groups.RemoveGroupUserAsync( roleId, oktaUserId);
    }

    /// <summary>
    /// Updates remote Auth0 profile's app metadata to include the given Revature account id. 
    /// </summary>
    /// <param name="oktaUserId"></param>
    /// <param name="newId"></param>
    /// <returns></returns>
    public async Task UpdateMetadataWithIdAsync(string oktaUserId, Guid newId)
    {
        var User = await Client.Users.GetUserAsync(oktaUserId);
        await Client.Users.UpdateUserAsync(User, oktaUserId, null);
    }
  }
}
