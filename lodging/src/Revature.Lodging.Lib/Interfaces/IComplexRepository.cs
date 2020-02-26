using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Revature.Lodging.Lib.Interfaces
{
    public interface IComplexRepository
    {

    /// <summary>
    ///   Gets all Complex objects from database.
    /// </summary>
    /// <returns> Collection of Complex objects </returns>
    public Task<IEnumerable<Complex>> GetComplexesAsync();

    /// <summary>
    ///   Gets a single Complex object by the given Complex ID from database.
    /// </summary>
    /// <param name="complexId"> Used to specify Complex object </param>
    /// <returns> A single Complex object </returns>
    public Task<Complex> GetComplexByIdAsync(int complexId);

    /// <summary>
    ///   Updates an existing Complex object from database.
    /// </summary>
    /// <param name="complex"> Used to indicate the modified Complex object </param>
    public void UpdateComplexAsync(Complex complex);

    /// <summary>
    ///   Deletes an existing Complex object from database.
    /// </summary>
    /// <param name="complexId"> Used to specify Complex object </param>
    public void DeleteComplexByIdAsync(int complexId);

    /// <summary>
    ///   Adds a single Complex object to database.
    /// </summary>
    /// <param name="newComplex"> Used to indicate a new Complex object </param>
    public void AddComplexAsync(Complex newComplex);
  }
}
