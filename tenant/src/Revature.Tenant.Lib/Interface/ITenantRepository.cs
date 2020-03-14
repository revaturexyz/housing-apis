using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revature.Tenant.Lib.Interface
{
  /// <summary>
  /// An interface that manages data access for tenants and their cars.
  /// </summary>
  public interface ITenantRepository : IDisposable
  {
    /// <summary>
    /// Adds a new tenant object as well as its associated properties.
    /// </summary>
    /// <param name="tenant">The tenant.</param>
    /// <exception cref="ArgumentNullException">Thrown when tenant is null.</exception>
    /// <exception cref="ArgumentException">Thrown when tenant info is incorrect.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task AddAsync(Models.Tenant tenant);

    /// <summary>
    /// Gets a tenant using their id.
    /// </summary>
    /// <param name="id">The ID of the tenant.</param>
    /// <returns>A tenant.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when id does not exist.</exception>
    public Task<Models.Tenant> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a list of all tenants.
    /// </summary>
    /// <returns>The collection of all tenants.</returns>
    public Task<ICollection<Models.Tenant>> GetAllAsync(string firstName = null, string lastName = null, string gender = null, Guid? trainingCenter = null);

    /// <summary>
    /// Deletes a tenant using their id.
    /// </summary>
    /// <param name="id">The ID of the tenant.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when id does not exist.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DeleteByIdAsync(Guid id);

    /// <summary>
    /// Updates values associated to a tenant.
    /// </summary>
    /// <param name="tenant">The tenant with changed values.</param>
    /// <exception cref="ArgumentException">Thrown when tenant info is incorrect.</exception>
    public void Put(Models.Tenant tenant);

    /// <summary>
    /// Gets all batches in a training center.
    /// </summary>
    /// <param name="trainingCenter">A Guid of a training center.</param>
    /// <returns>A list of batches.</returns>
    public Task<ICollection<Models.Batch>> GetBatchesAsync(Guid trainingCenter);

    /// <summary>
    /// This persists changes to database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SaveAsync();
  }
}
