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
    private readonly QueueClient _TenantQueue;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(ILogger<IdentityService> logger, IConfiguration configuration)
    {
      _TenantQueue = new QueueClient(configuration.GetConnectionString("ServiceBus"), configuration["Queues:TenantCUD"]);
      _logger = logger;
    }

    /// <summary>
    /// Method that creates new tenant accounts
    /// </summary>
    public async Task CreateAccount(Guid TenantId, string Email, string Name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = 0
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
    /// <summary>
    /// Method that deletes tenant accounts
    /// </summary>
    public async Task DeleteAccount(Guid TenantId, string Email, string Name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = 1
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
    /// <summary>
    /// Method that modifies existing tenant accounts
    /// </summary>
    public async Task UpdateAccount(Guid TenantId, string Email, string Name)
    {
      var tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = 2
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
  }
}
