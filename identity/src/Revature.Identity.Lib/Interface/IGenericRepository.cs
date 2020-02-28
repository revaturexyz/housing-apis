using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Revature.Account.Lib.Model;

namespace Revature.Account.Lib.Interface
{
  /// <summary>
  /// The interface-point for the data-access-layer's interaction with the business-logic-layer.
  /// </summary>
  public interface IGenericRepository
  {
    #region Provider

    public Task<Guid> GetProviderIdByEmailAsync(string providerEmail);

    public Task<ProviderAccount> GetProviderAccountByIdAsync(Guid providerId);

    public void AddProviderAccountAsync(ProviderAccount newAccount);

    public Task<bool> UpdateProviderAccountAsync(ProviderAccount providerAccount);

    public Task<bool> DeleteProviderAccountAsync(Guid providerId);

    #endregion

    #region Coordinator

    public Task<Guid> GetCoordinatorIdByEmailAsync(string coordinatorEmail);

    public Task<CoordinatorAccount> GetCoordinatorAccountByIdAsync(Guid coordinatorId);

    public Task<List<CoordinatorAccount>> GetAllCoordinatorAccountsAsync();

    public void AddCoordinatorAccount(CoordinatorAccount coordinator);

    public Task<bool> UpdateCoordinatorAccountAsync(CoordinatorAccount coordinator);

    public Task<bool> DeleteCoordinatorAccountAsync(Guid coordinatorId);

    #endregion

    #region Notification

    public Task<Notification> GetNotificationByIdAsync(Guid notificationId);

    public Task<List<Notification>> GetNotificationsByCoordinatorIdAsync(Guid coordinatorId);

    public void AddNotification(Notification newNofi);

    public Task<bool> UpdateNotificationAsync(Notification notification);

    public Task<bool> DeleteNotificationByIdAsync(Guid notificationId);

    #endregion

    #region UpdateAction

    public Task<UpdateAction> GetUpdateActionByIdAsync(Guid actionId);

    public void AddUpdateAction(UpdateAction action);

    public Task<bool> UpdateUpdateActionAsync(UpdateAction action);

    public Task<bool> DeleteUpdateActionByIdAsync(Guid actionId);
    #endregion

    #region Tenant

    public Task<Guid> GetTenantIdByEmailAsync(string TenantEmail);

    public Task<List<TenantAccount>> GetAllTenantsByProviderAsync(Guid ProviderId);

    public Task<List<TenantAccount>> GetAllTenantsByCoordinatorAsync(Guid CoordinatorId);

    public void AddTenantAccount(TenantAccount newTenant);

    public Task<TenantAccount> GetTenantAccountByIdAsync(Guid TenantId);

    public Task<bool> UpdateTenantAccountAsync(TenantAccount tenantAccount);

    public Task<bool> DeleteTenantAccountAsync(Guid tenantId);

    #endregion

    public Task SaveAsync();
  }
}
