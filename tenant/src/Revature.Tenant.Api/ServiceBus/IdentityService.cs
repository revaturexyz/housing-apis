using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Revature.Tenant.Api.ServiceBus
{
  public class IdentityService : IIdentityService
  {
    private readonly QueueClient _tenantQueue;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(ILogger<IdentityService> logger, IConfiguration configuration)
    {
      _tenantQueue = new QueueClient(configuration.GetConnectionString("ServiceBus"), configuration["Queues:TenantCUD"]);
      _logger = logger;
    }

    /// <summary>
    /// Method that creates new tenant accounts.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CreateAccount(Guid tenantId, string email, string name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = name,
        TenantId = tenantId,
        Email = email,
        OperationType = 0
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _tenantQueue.SendAsync(message);
    }

    /// <summary>
    /// Method that deletes tenant accounts.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteAccount(Guid tenantId, string email, string name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = name,
        TenantId = tenantId,
        Email = email,
        OperationType = 1
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _tenantQueue.SendAsync(message);
    }

    /// <summary>
    /// Method that modifies existing tenant accounts.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task UpdateAccount(Guid tenantId, string email, string name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = name,
        TenantId = tenantId,
        Email = email,
        OperationType = 2
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _tenantQueue.SendAsync(message);
    }
  }
}
