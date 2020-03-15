using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Lib.Interface
{
  public interface IComplexRepository
  {
    /// <summary>
    /// Create new single complex in the database by logic complex object.
    /// </summary>
    public Task<bool> CreateComplexAsync(Logic.Complex lComplex);

    /// <summary>
    /// Read all existed complices in the database.
    /// </summary>
    public Task<List<Logic.Complex>> ReadComplexListAsync();

    /// <summary>
    /// Read single Logic complex object from complex Id.
    /// </summary>
    public Task<Logic.Complex> ReadComplexByIdAsync(Guid complexId);

    /// <summary>
    /// Read single logic complex object from complex name and complex contact number.
    /// </summary>
    public Task<Logic.Complex> ReadComplexByNameAndNumberAsync(string name, string phone);

    /// <summary>
    /// Update existed single complex by passing logic complex object.
    /// </summary>
    public Task<bool> UpdateComplexAsync(Logic.Complex update);

    /// <summary>
    /// Delete existed single complex from database by specific complex Id.
    /// </summary>
    public Task<bool> DeleteComplexAsync(Guid complexId);

    /// <summary>
    /// Deletes a complex and deletes all rooms that is connected to that complex.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when room to be deleted isn't found in DB.</exception>
    public Task<List<Guid>> DeleteComplexRoomAsync(Guid complexId);

    /// <summary>
    /// Read complex list for specific provider from database by provider ID.
    /// </summary>
    public Task<List<Logic.Complex>> ReadComplexByProviderIdAsync(Guid providerId);
  }
}
