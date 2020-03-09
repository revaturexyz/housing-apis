using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Revature.Tenant.Lib.Models;


namespace Revature.Tenant.Api.ServiceBus
{
  public class IdentityService : IIdentityService
  {
    private readonly QueueClient _TenantQueue;
    private readonly ILogger<RoomService> _logger;

    public IdentityService(ILogger<RoomService> logger, IConfiguration configuration)
    {
      _TenantQueue = new QueueClient(configuration.GetConnectionString("ConnectionStrings: ServiceBus"), configuration["Queues:TenantCUD"]);
      _logger = logger;
    }
    /// <summary>
    /// Method that creates new tenant accounts
    /// </summary>
    public async Task CreateTenantAccount(Guid TenantId, string Email, string Name, int OperationType = 1)
    {
      Models.TenantAccountMessage tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = OperationType
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
    /// <summary>
    /// Method that deletes tenant accounts
    /// </summary>
    public async Task DeleteTenantAccount(Guid TenantId, string Email, string Name, int OperationType = 0)
    {
      Models.TenantAccountMessage tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = OperationType
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
    /// <summary>
    /// Method that modifies existing tenant accounts
    /// </summary>
    public async Task UpdateTenantAccount(Guid TenantId, string Email, string Name, int OperationType = 2)
    {
      Models.TenantAccountMessage tenantMessage = new Models.TenantAccountMessage
      {
        Name = Name,
        TenantId = TenantId,
        Email = Email,
        OperationType = OperationType
      };
      var data = JsonSerializer.Serialize(tenantMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));
      _logger.LogInformation("ServiceBus sending Create message: ", data);
      await _TenantQueue.SendAsync(message);
    }
  }
}
