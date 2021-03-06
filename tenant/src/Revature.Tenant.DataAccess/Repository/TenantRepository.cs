using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Revature.Tenant.DataAccess.Entities;
using Revature.Tenant.Lib.Interface;

namespace Revature.Tenant.DataAccess.Repository
{
  /// <summary>
  /// A repository for managing data access for tenant onjects and their cars.
  /// </summary>
  public sealed class TenantRepository : ITenantRepository
  {
    private readonly TenantContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantRepository"/> class given a suitable tenant data source.
    /// </summary>
    /// <param name="context">The data source.</param>
    /// <param name="mapper">The mapper.</param>
    public TenantRepository(TenantContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    /// <summary>
    /// Adds a new tenant object as well as its associated properties.
    /// </summary>
    /// <param name="tenant">The Tenant.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddAsync(Lib.Models.Tenant tenant)
    {
      var newTenant = _mapper.MapTenant(tenant);
      await _context.Tenant.AddAsync(newTenant);
    }

    /// <summary>
    /// Updates a new tenant object as well as its associated properties.
    /// </summary>
    /// <param name="tenant">The Tenant.</param>
    public void Put(Lib.Models.Tenant tenant)
    {
      var newTenant = _mapper.MapTenant(tenant);
      _context.Tenant.Update(newTenant);
      if (tenant.Car != null)
      {
        var newCar = _mapper.MapCar(tenant.Car);
        _context.Car.Update(newCar);
      }
    }

    /// <summary>
    /// Gets a tenant using their id.
    /// </summary>
    /// <param name="id">The ID of the tenant.</param>
    /// <returns>A tenant, including their Car and Batch, if applicable.</returns>
    public async Task<Lib.Models.Tenant> GetByIdAsync(Guid id)
    {
      var tenant = await _context.Tenant
        .Include(t => t.Car)
        .Include(t => t.Batch)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);

      if (tenant == null)
      {
        throw new ArgumentNullException($"Could not find ID: {id}");
      }

      return _mapper.MapTenant(tenant);
    }

    /// <summary>
    /// Gets a list of all tenants.
    /// </summary>
    /// <returns>The collection of all tenants, including their Car and Batch, if applicable.</returns>
    public async Task<ICollection<Lib.Models.Tenant>> GetAllAsync(string firstName = null, string lastName = null, string gender = null, Guid? trainingCenter = null)
    {
      var tenants = _context.Tenant
        .Include(t => t.Car)
        .Include(t => t.Batch)
        .AsNoTracking();

      if (!string.IsNullOrWhiteSpace(firstName))
      {
        tenants = tenants.Where(t => t.FirstName == firstName);
      }

      if (!string.IsNullOrWhiteSpace(lastName))
      {
        tenants = tenants.Where(t => t.LastName == lastName);
      }

      if (!string.IsNullOrWhiteSpace(gender))
      {
        tenants = tenants.Where(t => t.Gender == gender);
      }

      if (trainingCenter != null)
      {
        tenants = tenants.Where(t => t.TrainingCenter == trainingCenter);
      }

      return (await tenants.ToListAsync()).Select(_mapper.MapTenant).ToList();
    }

    /// <summary>
    /// Gets all batches in a training center.
    /// </summary>
    /// <param name="trainingCenter">A Guid of a training center.</param>
    /// <returns>A list of batches.</returns>
    public async Task<ICollection<Lib.Models.Batch>> GetBatchesAsync(Guid trainingCenter)
    {
      var batch = _context.Batch.Where(b => b.TrainingCenter == trainingCenter);
      return (await batch.ToListAsync())
        .Select(_mapper.MapBatch)
        .ToList();
    }

    /// <summary>
    /// Deletes a tenant using their id.
    /// </summary>
    /// <param name="id">The ID of the tenant.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteByIdAsync(Guid id)
    {
      var tenant = await _context.Tenant.FindAsync(id);

      _context.Remove(tenant);
    }

    /// <summary>
    /// Updates values associated to a tenant.
    /// </summary>
    /// <param name="tenant">The tenant with changed values.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task UpdateAsync(Lib.Models.Tenant tenant)
    {
      var currentTenant = await _context.Tenant.FindAsync(tenant.Id);

      if (currentTenant == null)
      {
        throw new InvalidOperationException("Invalid Tenant Id");
      }

      var newTenant = _mapper.MapTenant(tenant);
      _context.Entry(currentTenant).CurrentValues.SetValues(newTenant);
    }

    /// <summary>
    /// Takes in a tenant Id and checks if the tenant has a car.
    /// </summary>
    /// <param name="tenantId">tenant Id.</param>
    /// <returns>True if Tenant has Car, returns false if the Tenant has no car.</returns>
    public async Task<bool> HasCarAsync(Guid tenantId)
    {
      var currentTenant = await _context.Tenant.FindAsync(tenantId);
      if (currentTenant == null)
      {
        throw new InvalidOperationException($"Invalid Tenant Id {tenantId}");
      }
      else if (currentTenant.CarId == null || currentTenant.CarId == 0)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    /// <summary>
    /// This persists changes to data base.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
      _context.Dispose();
    }
  }
}
