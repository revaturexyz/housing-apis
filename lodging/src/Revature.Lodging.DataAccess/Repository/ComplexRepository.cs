using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Lib.Interface;
using Entity = Revature.Lodging.DataAccess.Entities;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.DataAccess.Repository
{
  public class ComplexRepository : IComplexRepository
  {
    private readonly Entity.LodgingDbContext _context;
    private readonly IRoomRepository _roomRepo;
    private readonly ILogger<ComplexRepository> _log;

    public ComplexRepository(Entity.LodgingDbContext context, IRoomRepository roomRepo, ILogger<ComplexRepository> logger)
    {
      _context = context;
      _roomRepo = roomRepo ?? throw new NullReferenceException("Room repository cannot be null." + nameof(roomRepo));
      _log = logger;
    }

    /// <summary>
    /// Create new single complex in the database by logic complex object.
    /// </summary>
    public async Task<bool> CreateComplexAsync(Logic.Complex lComplex)
    {
      var complex = Mapper.Map(lComplex);

      await _context.AddAsync(complex);
      await _context.SaveChangesAsync();
      _log.LogInformation("new complex: {complexId} was inserted ", lComplex.Id);

      return true;
    }

    /// <summary>
    /// Read all existed complices in the database.
    /// </summary>
    /// <exception cref="ArgumentNullException">list of complex not found.</exception>
    public async Task<List<Logic.Complex>> ReadComplexListAsync()
    {
      try
      {
        var complices = await _context.Complex.ToListAsync();

        return complices.Select(Mapper.Map).ToList();
      }
      catch (Exception ex)
      {
        _log.LogError("{ex} couldn't find list of complices", ex);
        throw;
      }
    }

    /// <summary>
    /// Read single Logic complex object from complex Id.
    /// </summary>
    /// <exception cref="ArgumentNullException">complex not found by id.</exception>
    public async Task<Logic.Complex> ReadComplexByIdAsync(Guid complexId)
    {
      try
      {
        var complexFind = await _context.Complex.FindAsync(complexId);
        return Mapper.Map(complexFind);
      }
      catch (ArgumentException ex)
      {
        _log.LogError("{ex}: couldn't find specific complices with id: {complexId}", ex, complexId);
        throw;
      }
    }

    /// <summary>
    /// Read single logic complex object from complex name and complex contact number.
    /// </summary>
    /// <exception cref="ArgumentNullException">list of complex not found.</exception>
    public async Task<Logic.Complex> ReadComplexByNameAndNumberAsync(string name, string phone)
    {
      try
      {
        var complex = await _context.Complex
          .Where(c => c.ComplexName == name && c.ContactNumber == phone)
          .AsNoTracking()
          .FirstOrDefaultAsync();
        return Mapper.Map(complex);
      }
      catch (ArgumentException ex)
      {
        _log.LogError(ex, "couldn't find specific complex with name: {name} and phone: {phone}", name, phone);
        throw;
      }
    }

    /// <summary>
    /// Update existed single complex by passing logic complex object.
    /// </summary>
    /// <exception cref="ArgumentNullException">complex not found.</exception>
    public async Task<bool> UpdateComplexAsync(Logic.Complex update)
    {
      try
      {
        var origin = await _context.Complex.FindAsync(update.Id);

        if (update.ComplexName != null)
        {
          origin.ComplexName = update.ComplexName;
        }

        if (update.ContactNumber != null)
        {
          origin.ContactNumber = update.ContactNumber;
        }

        await _context.SaveChangesAsync();
        _log.LogInformation("{complexId} was updated", update.Id);

        return true;
      }
      catch (ArgumentException ex)
      {
        _log.LogError("{ex}comlex id: {ComplexId} update failed", ex, update.Id);
        throw;
      }
    }

    /// <summary>
    /// Delete existed single complex from database by specific complex Id.
    /// </summary>
    /// <exception cref="ArgumentNullException">complex not found.</exception>
    public async Task<bool> DeleteComplexAsync(Guid complexId)
    {
      try
      {
        var target = await _context.Complex.FindAsync(complexId);

        _context.Remove(target);
        await _context.SaveChangesAsync();
        _log.LogInformation("target: {complexId} was deleted", target.Id);

        return true;
      }
      catch (ArgumentException ex)
      {
        _log.LogError(ex, "complex id: {complexId} failed to delete", complexId);
        throw;
      }
    }

    /// <summary>
    /// Read complex list for specific provider from database by provider ID.
    /// </summary>
    /// <exception cref="ArgumentNullException">list of complex not found.</exception>
    public async Task<List<Logic.Complex>> ReadComplexByProviderIdAsync(Guid providerId)
    {
      try
      {
        var complices = await _context.Complex.Where(c => c.ProviderId == providerId).ToListAsync();

        return complices.Select(Mapper.Map).ToList();
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "complices of provider Id: {pId} were not found", providerId);
        throw;
      }
    }

    public async Task<List<Guid>> DeleteComplexRoomAsync(Guid complexId)
    {
      var roomEntity = await _context.Room.Where(r => r.ComplexId == complexId).Select(r => r.Id).ToListAsync();

      foreach (var r in roomEntity)
      {
        await _roomRepo.DeleteRoomAsync(r);
      }

      await _context.SaveChangesAsync();

      return roomEntity;
    }
  }
}
