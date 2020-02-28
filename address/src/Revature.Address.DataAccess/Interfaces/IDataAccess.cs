using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revature.Address.DataAccess.Interfaces
{

  /// <summary>
  /// DataAccess interface for dependency injection, specifies the
  /// retrieval, insertion, and deletion of addresses from the database
  /// </summary>
  public interface IDataAccess : IDisposable
  {
    public Task<bool> AddAddressAsync(Revature.Address.Lib.Address address);
    public Task<ICollection<Revature.Address.Lib.Address>> GetAddressAsync(Guid? id = null, Revature.Address.Lib.Address address = null);
    public Task<bool> DeleteAddressAsync(Guid? id);
    public Task SaveAsync();
  }
}
