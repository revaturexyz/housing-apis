using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Lib.Interface;
using Revature.Lodging.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity = Revature.Lodging.DataAccess.Entities;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.DataAccess.Repository
{
  class AmenityRepository : IAmenityRepository
  {
    private readonly Entity.LodgingDbContext _context;
    private readonly ILogger<AmenityRepository> _log;

    public AmenityRepository(Entity.LodgingDbContext context, ILogger<AmenityRepository> logger)
    {
      _context = context;
      _log = logger;
    }

    public async Task<bool> CreateAmenityAsync(Amenity amenity)
    {
      var newAmenity = Mapper.Map(amenity);

      await _context.AddAsync(newAmenity);
      await _context.SaveChangesAsync();
      _log.LogInformation("new Amenity: {amenity.AmenityType} was added", amenity.AmenityType);

      return true;
    }

    public async Task<bool> CreateAmenityComplexAsync(AmenityComplex ac)
    {
      var amenityComplex = Mapper.Map(ac);

      await _context.AddAsync(amenityComplex);
      await _context.SaveChangesAsync();
      _log.LogInformation("new amenity for complex: {AmenityComplexId} was added", ac.Id);

      return true;
    }

    public async Task<bool> CreateAmenityRoomAsync(AmenityRoom ar)
    {
      var amenityRoom = Mapper.Map(ar);

      await _context.AddAsync(amenityRoom);
      await _context.SaveChangesAsync();
      _log.LogInformation("new amenity of room id: {roomId}", ar.RoomId);

      return true;
    }

    public async Task<bool> DeleteAmenityAsync(Amenity amenity)
    {
      try
      {
        var dAmenity = await _context.Amenity.FindAsync(amenity.Id);

        _context.Remove(dAmenity);

        await _context.SaveChangesAsync();
        _log.LogInformation("amenity: {dAmenity.AmenityId} {dAmenity.AmenityType} is deleted", dAmenity.Id, dAmenity.AmenityType);

        return true;
      }
      catch (InvalidOperationException ex)
      {
        _log.LogWarning(ex, "Unable to delete the amenity.");
        throw;
      }
    }

    public async Task<bool> DeleteAmenityComplexAsync(Guid complexId)
    {
      try
      {
        _context.AmenityComplex.RemoveRange(_context.AmenityComplex.Where(ar => ar.ComplexId == complexId));

        await _context.SaveChangesAsync();

        return true;
      }
      catch (Exception ex)
      {
        _log.LogWarning("{ex}: couldn't find such room with complex id: {complexId}", ex, complexId);
        throw;
      }
    }

    public async Task<bool> DeleteAmenityRoomAsync(Guid roomId)
    {
      try
      {
        _context.AmenityRoom.RemoveRange(_context.AmenityRoom.Where(ar => ar.RoomId == roomId));

        await _context.SaveChangesAsync();
        _log.LogInformation("AmenityRooms with room Id: {roomId} were deleted", roomId);

        return true;
      }
      catch (ArgumentException ex)
      {
        _log.LogError("{ex}: couldn't find such room with room id: {roomId}", ex, roomId);
        throw;
      }
    }

    public async Task<List<Amenity>> ReadAmenityListAsync()
    {
      try
      {
        var amenities = await _context.Amenity.ToListAsync();

        return amenities.Select(Mapper.Map).ToList();
      }
      catch (ArgumentException ex)
      {
        _log.LogError(ex, "couldn't find list of amenities in the database");
        throw;
      }
    }

    public async Task<List<Amenity>> ReadAmenityListByComplexIdAsync(Guid complexId)
    {
      try
      {
        var amenityComplices = await _context.AmenityComplex
          .Where(a => a.ComplexId == complexId).ToListAsync();

        var amenities = new List<Logic.Amenity>();
        foreach (var ac in amenityComplices)
        {
          amenities.Add(Mapper.Map(await _context.Amenity.FindAsync(ac.AmenityId)));
          _log.LogInformation("amenity: {ac.AmenityId} was found and added", ac.AmenityId);
        }

        return amenities;
      }
      catch (ArgumentException ex)
      {
        _log.LogError("{ex}: amenities of complex were not found", ex);
        throw;
      }
    }

    public async Task<List<Amenity>> ReadAmenityListByRoomIdAsync(Guid roomId)
    {
      try
      {
        var amenityRooms = await _context.AmenityRoom
          .Where(a => a.RoomId == roomId).AsNoTracking().ToListAsync();

        var amenities = new List<Logic.Amenity>();
        foreach (var ac in amenityRooms)
        {
          amenities.Add(Mapper.Map(await _context.Amenity.FindAsync(ac.AmenityId)));
        }

        return amenities;
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "amenities for room id: {roomId} were not found", roomId);
        throw;
      }
    }

    public async Task<bool> UpdateAmenityAsync(Amenity amenity)
    {
      try
      {
        var eAmenity = await _context.Amenity.FindAsync(amenity.Id);

        if (amenity.AmenityType != null)
        {
          eAmenity.AmenityType = amenity.AmenityType;
        }
        if (amenity.Description != null)
        {
          eAmenity.Description = amenity.Description;
        }

        await _context.SaveChangesAsync();
        _log.LogInformation("amenity: {amenity.AmenityId} {amenity.AmenityType} was updated"
                                      , amenity.Id, amenity.AmenityType);

        return true;
      }
      catch (ArgumentException ex)
      {
        _log.LogWarning(ex, "Unable to update the amenity.");
        throw;
      }
    }
  }
}
