using Revature.Lodging.DataAccess;
using System;
using Xunit;
using Entity = Revature.Lodging.DataAccess.Entities;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Tests.DataTests
{
  public class MapperTest
  {
    /// <summary>
    /// This test is to test ComplextoE in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void ComplextoETest()
    {
      var cId = Guid.NewGuid();
      var aId = Guid.NewGuid();
      var pId = Guid.NewGuid();

      var c = new Logic.Complex
      {
        Id = cId,
        AddressId = aId,
        ProviderId = pId,
        ComplexName = "Liv+",
        ContactNumber = "(123)456-7890"
      };

      var ce = Mapper.Map(c);

      Assert.Equal(cId, ce.Id);
      Assert.Equal(aId, ce.AddressId);
      Assert.Equal(pId, ce.ProviderId);
      Assert.Equal("Liv+", ce.ComplexName);
      Assert.Equal("(123)456-7890", ce.ContactNumber);
    }

    /// <summary>
    /// This test is to test EtoComplex in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void EtoComplexTest()
    {
      var cId = Guid.NewGuid();
      var aId = Guid.NewGuid();
      var pId = Guid.NewGuid();

      var c = new Entity.Complex
      {
        Id = cId,
        AddressId = aId,
        ProviderId = pId,
        ComplexName = "Liv+",
        ContactNumber = "(123)456-7890"
      };

      var cl = Mapper.Map(c);

      Assert.Equal(cId, cl.Id);
      Assert.Equal(aId, cl.AddressId);
      Assert.Equal(pId, cl.ProviderId);
      Assert.Equal("Liv+", cl.ComplexName);
      Assert.Equal("(123)456-7890", cl.ContactNumber);
    }

    /// <summary>
    /// This test is to test AmenityRoomtoE in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void AmenityRoomtoETest()
    {
      var arId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var rId = Guid.NewGuid();

      var ar = new Logic.RoomAmenity
      {
        Id = arId,
        AmenityId = amId,
        RoomId = rId
      };

      var eAr = Mapper.Map(ar);

      Assert.Equal(arId, eAr.Id);
      Assert.Equal(amId, eAr.AmenityId);
      Assert.Equal(rId, eAr.RoomId);
    }

    /// <summary>
    /// This test is to test EtoAmenityRoom in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void EtoAmenityRoom()
    {
      var arId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var rId = Guid.NewGuid();

      var ar = new Entity.RoomAmenity
      {
        Id = arId,
        AmenityId = amId,
        RoomId = rId
      };

      var lAr = Mapper.Map(ar);

      Assert.Equal(arId, lAr.Id);
      Assert.Equal(amId, lAr.AmenityId);
      Assert.Equal(rId, lAr.RoomId);
    }

    /// <summary>
    /// This test is to test AmenityComplextoE in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void AmenityComplextoETest()
    {
      var acId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var cId = Guid.NewGuid();

      var ac = new Logic.ComplexAmenity
      {
        Id = acId,
        AmenityId = amId,
        ComplexId = cId
      };

      var eAc = Mapper.Map(ac);

      Assert.Equal(acId, eAc.Id);
      Assert.Equal(amId, eAc.AmenityId);
      Assert.Equal(cId, eAc.ComplexId);
    }

    /// <summary>
    /// This test is to test EtoAmenityComplex in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void EtoAmenityComplexTest()
    {
      var acId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var cId = Guid.NewGuid();

      var ac = new Entity.ComplexAmenity
      {
        Id = acId,
        AmenityId = amId,
        ComplexId = cId
      };

      var lAc = Mapper.Map(ac);

      Assert.Equal(acId, lAc.Id);
      Assert.Equal(amId, lAc.AmenityId);
      Assert.Equal(cId, lAc.ComplexId);
    }

    /// <summary>
    /// This test is to test AmenitytoE in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void AmenitytoETest()
    {
      var amId = Guid.NewGuid();

      var a = new Logic.Amenity
      {
        Id = amId,
        AmenityType = "Fridge",
        Description = "To freeze items"
      };

      var ea = Mapper.Map(a);

      Assert.Equal(amId, ea.Id);
      Assert.Equal("Fridge", ea.AmenityType);
      Assert.Equal("To freeze items", ea.Description);
    }

    /// <summary>
    /// This test is to test EtoAmenity in DataAccess.Mapper.cs.
    /// </summary>
    [Fact]
    public void EtoAmenityTest()
    {
      var amId = Guid.NewGuid();

      var a = new Entity.Amenity
      {
        Id = amId,
        AmenityType = "Fridge",
        Description = "To freeze items"
      };

      var la = Mapper.Map(a);

      Assert.Equal(amId, la.Id);
      Assert.Equal("Fridge", la.AmenityType);
      Assert.Equal("To freeze items", la.Description);
    }
  }
}
