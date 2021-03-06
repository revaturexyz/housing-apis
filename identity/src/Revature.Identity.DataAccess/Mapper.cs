using System.Linq;

namespace Revature.Identity.DataAccess
{
  /// <summary>
  /// Maps between the business logic and data access layers.
  /// </summary>
  public class Mapper : IMapper
  {
    /// <summary>
    /// Maps DB tenant to logic tenant. Maps related coordinator and Status as well.
    /// </summary>
    public Lib.Model.TenantAccount MapTenant(Entities.TenantAccount tenant)
    {
      return new Lib.Model.TenantAccount
      {
        TenantId = tenant.TenantId,
        Name = tenant.Name,
        Email = tenant.Email
      };
    }

    public Entities.TenantAccount MapTenant(Lib.Model.TenantAccount tenant)
    {
      return new Entities.TenantAccount
      {
        TenantId = tenant.TenantId,
        Name = tenant.Name,
        Email = tenant.Email
      };
    }

    /// <summary>
    /// Maps DB Provider to logic Provider. Maps related coordinator and Status as well.
    /// </summary>
    public Lib.Model.ProviderAccount MapProvider(Entities.ProviderAccount provider)
    {
      return new Lib.Model.ProviderAccount
      {
        ProviderId = provider.ProviderId,
        CoordinatorId = provider.CoordinatorId,
        Name = provider.Name,
        Email = provider.Email,
        Status = new Lib.Model.Status { StatusText = provider.StatusText },
        AccountCreatedAt = provider.AccountCreatedAt,
        AccountExpiresAt = provider.AccountExpiresAt
      };
    }

    public Entities.ProviderAccount MapProvider(Lib.Model.ProviderAccount provider)
    {
      return new Entities.ProviderAccount
      {
        ProviderId = provider.ProviderId,
        CoordinatorId = provider.CoordinatorId,
        Name = provider.Name,
        Email = provider.Email,
        StatusText = provider.Status.StatusText,
        AccountCreatedAt = provider.AccountCreatedAt,
        AccountExpiresAt = provider.AccountExpiresAt
      };
    }

    /// <summary>
    /// Maps DB Coordinator to logic Coordinator. Maps related list of outstanding
    /// Notifications as well.
    /// </summary>
    public Lib.Model.CoordinatorAccount MapCoordinator(Entities.CoordinatorAccount coordinator)
    {
      return new Lib.Model.CoordinatorAccount
      {
        CoordinatorId = coordinator.CoordinatorId,
        Name = coordinator.Name,
        Email = coordinator.Email,
        TrainingCenterName = coordinator.TrainingCenterName,
        TrainingCenterAddress = coordinator.TrainingCenterAddress,
        Notifications = coordinator.Notifications.Select(MapNotification).ToList()
      };
    }

    public Entities.CoordinatorAccount MapCoordinator(Lib.Model.CoordinatorAccount coordinator)
    {
      return new Entities.CoordinatorAccount
      {
        CoordinatorId = coordinator.CoordinatorId,
        Name = coordinator.Name,
        Email = coordinator.Email,
        TrainingCenterName = coordinator.TrainingCenterName,
        TrainingCenterAddress = coordinator.TrainingCenterAddress
      };
    }

    /// <summary>
    /// Maps DB Notification to logic Notification. Maps Coordinator, Provider, and
    /// UpdateAction as well.
    /// </summary>
    public Lib.Model.Notification MapNotification(Entities.Notification nofi)
    {
      return new Lib.Model.Notification
      {
        NotificationId = nofi.NotificationId,
        ProviderId = nofi.ProviderId,
        CoordinatorId = nofi.CoordinatorId,
        UpdateAction = MapUpdateAction(nofi.UpdateAction),
        Status = new Lib.Model.Status { StatusText = nofi.StatusText },
        CreatedAt = nofi.CreatedAt
      };
    }

    public Entities.Notification MapNotification(Lib.Model.Notification nofi)
    {
      return new Entities.Notification
      {
        NotificationId = nofi.NotificationId,
        ProviderId = nofi.ProviderId,
        CoordinatorId = nofi.CoordinatorId,
        UpdateActionId = nofi.UpdateAction.UpdateActionId,
        StatusText = nofi.Status.StatusText,
        CreatedAt = nofi.CreatedAt
      };
    }

    public Lib.Model.UpdateAction MapUpdateAction(Entities.UpdateAction action)
    {
      return new Lib.Model.UpdateAction
      {
        UpdateActionId = action.UpdateActionId,
        NotificationId = action.NotificationId,
        UpdateType = action.UpdateType,
        SerializedTarget = action.SerializedTarget
      };
    }

    public Entities.UpdateAction MapUpdateAction(Lib.Model.UpdateAction action)
    {
      return new Entities.UpdateAction
      {
        UpdateActionId = action.UpdateActionId,
        NotificationId = action.NotificationId,
        UpdateType = action.UpdateType,
        SerializedTarget = action.SerializedTarget
      };
    }
  }
}
