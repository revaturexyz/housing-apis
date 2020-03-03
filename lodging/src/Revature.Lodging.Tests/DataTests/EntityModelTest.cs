using System;
using Xunit;
using Entity = Revature.Lodging.DataAccess.Entities;

namespace Revature.Lodging.Tests.DataTests
{
  public class EntityModelTest
  {
    /// <summary>
    /// This test is to test Amenity in Entity Model
    /// </summary>
    [Fact]
    public void AmenityTest()
    {
      var amId = Guid.NewGuid();
      var amenity = new Entity.Amenity
      {
        Id = amId,
        AmenityType = "fridge",
        Description = "to freeze items"
      };

      Assert.Equal(amId, amenity.Id);
      Assert.Equal("fridge", amenity.AmenityType);
      Assert.Equal("to freeze items", amenity.Description);
    }

    /// <summary>
    /// This test is to test AmenityComplex in Entity Model
    /// </summary>
    [Fact]
    public void AmenityComplexTest()
    {
      var acId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var guid1 = Guid.NewGuid();

      var ac = new Entity.AmenityComplex
      {
        Id = acId,
        AmenityId = amId,
        ComplexId = guid1
      };

      Assert.Equal(acId, ac.Id);
      Assert.Equal(amId, ac.AmenityId);
      Assert.Equal(guid1, ac.ComplexId);
    }

    /// <summary>
    /// This test is to test AmenityRoom in Entity Model
    /// </summary>
    [Fact]
    public void AmenityRoomTest()
    {
      var arId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var guid = Guid.NewGuid();

      var ar = new Entity.AmenityRoom
      {
        Id = arId,
        AmenityId = amId,
        RoomId = guid
      };

      Assert.Equal(arId, ar.Id);
      Assert.Equal(amId, ar.AmenityId);
      Assert.Equal(guid, ar.RoomId);
    }

    /// <summary>
    /// This test is to test Complex in Entity Model
    /// </summary>
    [Fact]
    public void ComplexTest()
    {
      var cId = Guid.NewGuid();
      var aId = Guid.NewGuid();
      var pId = Guid.NewGuid();

      var complex = new Entity.Complex
      {
        Id = cId,
        AddressId = aId,
        ProviderId = pId,
        ComplexName = "Liv+",
        ContactNumber = "(123)456-7890"
      };

      Assert.Equal(cId, complex.Id);
      Assert.Equal(aId, complex.AddressId);
      Assert.Equal(pId, complex.ProviderId);
      Assert.Equal("Liv+", complex.ComplexName);
      Assert.Equal("(123)456-7890", complex.ContactNumber);
    }
  }
}
