using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Revature.Address.DataAccess.Entities;
using Revature.Address.DataAccess.Interfaces;
using Revature.Address.Lib.Interfaces;

namespace Revature.Address.DataAccess
{
  /// <summary>
  /// Contain methods for inserting, retrieving, and deleting
  /// information from the database.
  /// </summary>
  public sealed class DataAccess : IDataAccess
  {
    private readonly IMapper _mapper;
    private readonly AddressDbContext _context;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccess"/> class.
    /// </summary>
    /// <param name="context">DbContext for accessing the database.</param>
    public DataAccess(AddressDbContext context, IMapper mapper, ILogger<DataAccess> logger)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Checks if address already exists in database;
    /// if it doesn't it converts it and adds it to the database.
    /// </summary>
    public async Task<bool> AddAddressAsync(Lib.Address address)
    {
      try
      {
        var newAddress = (await GetAddressAsync(address: address)).FirstOrDefault();
        if (newAddress == null)
        {
          await _context.AddAsync(_mapper.MapAddress(address));
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("{0}", ex);
        _logger.LogError($"Address Id: {address.Id} failed to add to database");
        return false;
      }

      return true;
    }

    /// <summary>
    /// Given either an id or an address it retrieves an address from the database.
    /// </summary>
    /// <returns>An address.</returns>
    public async Task<ICollection<Lib.Address>> GetAddressAsync(Guid? id = null, Lib.Address address = null)
    {
      var addresses = await _context.Addresses.AsNoTracking().ToListAsync();

      if (id != null)
        addresses = addresses.Where(a => a.Id == id).ToList();
      if (address != null)
      {
        addresses = addresses.Where(a => a.Street == address.Street && a.City == address.City && a.State == address.State && a.ZipCode == address.ZipCode && a.Country == address.Country).ToList();
      }

      return addresses.Select(_mapper.MapAddress).ToList();
    }

    /// <summary>
    /// Given an id, it deletes the corresponding address from the database.
    /// </summary>
    public async Task<bool> DeleteAddressAsync(Guid? id)
    {
      if (await _context.Addresses.FindAsync(id) is Entities.Address item)
      {
        _context.Addresses.Remove(item);
        return true;
      }

      return false;
    }

    /// <summary>
    /// Saves changes made to the database.
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
