using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Revature.Identity.Lib.Interface;
using Revature.Identity.Lib.Model;

namespace Revature.Identity.DataAccess.Repositories
{
  /// <summary>
  /// Describes the methods used to get and manipulate data between the database and business layer.
  /// </summary>
  public class GenericRepository : IGenericRepository
  {
    // the database
    private readonly IdentityDbContext _context;

    // the mapper tool
    private readonly IMapper _mapper;

    // constructor
    public GenericRepository(IdentityDbContext db, IMapper mapper)
    {
      // inject the database
      _context = db ?? throw new ArgumentNullException(nameof(db), "Context cannot be null.");

      // instantiate the mapper
      _mapper = mapper;
    }

    public async Task<Guid> GetProviderIdByEmailAsync(string providerEmail)
    {
      var provider = await _context.ProviderAccount
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Email == providerEmail);
      return provider != null ? provider.ProviderId : Guid.Empty;
    }

    /// <summary>
    /// Get the Provider by GUID.
    /// </summary>
    public async Task<ProviderAccount> GetProviderAccountByIdAsync(Guid providerId)
    {
      var provider = await _context.ProviderAccount
        .AsNoTracking()
        .Include(p => p.Coordinator)
        .FirstOrDefaultAsync(p => p.ProviderId == providerId);
      return provider != null ? _mapper.MapProvider(provider) : null;
    }

    /// <summary>
    /// Add a Provider to the database.
    /// </summary>
    public void AddProviderAccountAsync(ProviderAccount newAccount)
    {
      var newEntity = _mapper.MapProvider(newAccount);
      _context.Add(newEntity);
    }

    /// <summary>
    /// Update a Provider's account on the database.
    /// </summary>
    public async Task<bool> UpdateProviderAccountAsync(ProviderAccount providerAccount)
    {
      var existingEntity = await _context.ProviderAccount.FirstOrDefaultAsync(p => p.ProviderId == providerAccount.ProviderId);
      if (existingEntity == null)
        return false;

      var updatedEntity = _mapper.MapProvider(providerAccount);
      _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
      return true;
    }

    /// <summary>
    /// Delete a provider's account from the database.
    /// </summary>
    public async Task<bool> DeleteProviderAccountAsync(Guid providerId)
    {
      var entityToBeRemoved = await _context.ProviderAccount.FirstOrDefaultAsync(p => p.ProviderId == providerId);
      if (entityToBeRemoved == null)
        return false;

      _context.Remove(entityToBeRemoved);
      return true;
    }

    public async Task<Guid> GetCoordinatorIdByEmailAsync(string coordinatorEmail)
    {
      var coordinator = await _context.CoordinatorAccount
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Email == coordinatorEmail);
      return coordinator != null ? coordinator.CoordinatorId : Guid.Empty;
    }

    /// <summary>
    /// Get a Coordinator's account from the databse.
    /// </summary>
    public async Task<CoordinatorAccount> GetCoordinatorAccountByIdAsync(Guid coordinatorId)
    {
      var coordinator = await _context.CoordinatorAccount
        .AsNoTracking()
        .Include(c => c.Notifications)
        .FirstOrDefaultAsync(p => p.CoordinatorId == coordinatorId);
      return coordinator != null ? _mapper.MapCoordinator(coordinator) : null;
    }

    /// <summary>
    /// Gets all available coordinator accounts.
    /// </summary>
    public async Task<List<CoordinatorAccount>> GetAllCoordinatorAccountsAsync()
    {
      var coordinators = await _context.CoordinatorAccount
        .AsNoTracking()
        .Include(c => c.Notifications)
        .ToListAsync();
      return coordinators.Select(_mapper.MapCoordinator).ToList();
    }

    public void AddCoordinatorAccount(CoordinatorAccount coordinator)
    {
      var newEntity = _mapper.MapCoordinator(coordinator);
      _context.Add(newEntity);
    }

    public async Task<bool> UpdateCoordinatorAccountAsync(CoordinatorAccount coordinator)
    {
      var existingEntity = await _context.CoordinatorAccount.FindAsync(coordinator.CoordinatorId);
      if (existingEntity == null)
        return false;

      var updatedEntity = _mapper.MapCoordinator(coordinator);
      _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
      return true;
    }

    public async Task<bool> DeleteCoordinatorAccountAsync(Guid coordinatorId)
    {
      var entityToBeRemoved = await _context.CoordinatorAccount.FindAsync(coordinatorId);
      if (entityToBeRemoved == null)
        return false;

      _context.Remove(entityToBeRemoved);
      return true;
    }

    /// <summary>
    /// Get a notification by its GUID.
    /// </summary>
    public async Task<Notification> GetNotificationByIdAsync(Guid notificationId)
    {
      var notification = await _context.Notification
        .AsNoTracking()
        .Include(n => n.Coordinator)
        .Include(n => n.Provider)
        .Include(n => n.UpdateAction)
        .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
      return notification != null ? _mapper.MapNotification(notification) : null;
    }

    /// <summary>
    /// Get a list of notifications based on the attached coordinator.
    /// </summary>
    public async Task<List<Notification>> GetNotificationsByCoordinatorIdAsync(Guid coordinatorId)
    {
      var notification = await _context.Notification
        .Include(n => n.Coordinator)
        .Include(n => n.Provider)
        .Include(n => n.UpdateAction)
        .Where(p => p.CoordinatorId == coordinatorId).ToListAsync();
      return notification?.Select(_mapper.MapNotification).ToList();
    }

    /// <summary>
    /// Add a new notification to the database.
    /// </summary>
    public void AddNotification(Notification newNofi)
    {
      newNofi.UpdateAction.NotificationId = newNofi.NotificationId;
      AddUpdateAction(newNofi.UpdateAction);

      var addedNotifiaction = _mapper.MapNotification(newNofi);
      _context.Add(addedNotifiaction);
    }

    /// <summary>
    /// Delete an individual notification from the database.
    /// </summary>
    public async Task<bool> DeleteNotificationByIdAsync(Guid notificationId)
    {
      var entityToBeRemoved = await _context.Notification.FirstOrDefaultAsync(n => n.NotificationId == notificationId);
      if (entityToBeRemoved == null)
        return false;

      if (!await DeleteUpdateActionByIdAsync(entityToBeRemoved.UpdateActionId))
        return false;

      _context.Remove(entityToBeRemoved);
      return true;
    }

    /// <summary>
    /// Update an individual notification stored on the database.
    /// </summary>
    public async Task<bool> UpdateNotificationAsync(Notification notification)
    {
      var existingEntity = await _context.Notification.FirstOrDefaultAsync(n => n.NotificationId == notification.NotificationId);
      if (existingEntity == null)
        return false;

      if (!await UpdateUpdateActionAsync(notification.UpdateAction))
        return false;

      var updatedEntity = _mapper.MapNotification(notification);
      _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
      return true;
    }

    public async Task<UpdateAction> GetUpdateActionByIdAsync(Guid actionId)
    {
      var action = await _context.UpdateAction
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.UpdateActionId == actionId);
      return action != null ? _mapper.MapUpdateAction(action) : null;
    }

    public void AddUpdateAction(UpdateAction action)
    {
      var newEntity = _mapper.MapUpdateAction(action);
      _context.Add(newEntity);
    }

    public async Task<bool> UpdateUpdateActionAsync(UpdateAction action)
    {
      var existingEntity = await _context.UpdateAction.FirstOrDefaultAsync(u => u.UpdateActionId == action.UpdateActionId);
      if (existingEntity == null)
        return false;

      var updatedEntity = _mapper.MapUpdateAction(action);
      _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
      return true;
    }

    public async Task<bool> DeleteUpdateActionByIdAsync(Guid actionId)
    {
      var entityToBeRemoved = await _context.UpdateAction.FirstOrDefaultAsync(u => u.UpdateActionId == actionId);
      if (entityToBeRemoved == null)
        return false;

      _context.Remove(entityToBeRemoved);
      return true;
    }

    public async Task<Guid> GetTenantIdByEmailAsync(string tenantEmail)
    {
      var tenant = await _context.TenantAccount
    .AsNoTracking()
    .FirstOrDefaultAsync(c => c.Email == tenantEmail);
      return tenant != null ? tenant.TenantId : Guid.Empty;
    }

    public void AddTenantAccount(TenantAccount newTenant)
    {
      var newEntity = _mapper.MapTenant(newTenant);
      _context.Add(newEntity);
    }

    public async Task<TenantAccount> GetTenantAccountByIdAsync(Guid tenantId)
    {
      var tenant = await _context.TenantAccount
  .AsNoTracking()
  .FirstOrDefaultAsync(p => p.TenantId == tenantId);
      return tenant != null ? _mapper.MapTenant(tenant) : null;
    }

    public async Task<bool> UpdateTenantAccountAsync(TenantAccount tenantAccount)
    {
      var existingEntity = await _context.CoordinatorAccount.FindAsync(tenantAccount.TenantId);
      if (existingEntity == null)
        return false;

      var updatedEntity = _mapper.MapTenant(tenantAccount);
      _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
      return true;
    }

    public async Task<bool> DeleteTenantAccountAsync(Guid tenantId)
    {
      var entityToBeRemoved = await _context.TenantAccount.FindAsync(tenantId);
      if (entityToBeRemoved == null)
        return false;

      _context.Remove(entityToBeRemoved);
      return true;
    }

    /// <summary>
    /// Save changes in context to the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
