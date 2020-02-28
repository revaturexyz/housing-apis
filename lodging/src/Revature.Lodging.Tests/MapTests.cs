using Revature.Lodging.Lib.Models;
using Revature.Lodging.DataAccess;
using System;
using Xunit;

namespace Revature.Lodging.Tests
{
  public class MapTests
  {
    Mapper mapObject = new Mapper();

    [Fact]
    public void MapAmentiyToEntity()
    {
      Guid temp = Guid.NewGuid();
      Amenity subject = new Amenity()
      {
        AmenityId = temp,
        AmenityType = "TV",
        Description = "100 channels."
      };
      DataAccess.Entities.Amenity mappedSubject = mapObject.Map(subject);
      Assert.Equal(temp, mappedSubject.AmenityId);
      Assert.Equal("TV", mappedSubject.AmenityType);
      Assert.Equal("100 channels.", mappedSubject.Description);
    }

    [Fact]
    public void MapAmentiyToLib()
    {
      Guid temp = Guid.NewGuid();
      DataAccess.Entities.Amenity subject = new DataAccess.Entities.Amenity()
      {
        AmenityId = temp,
        AmenityType = "Balcony",
        Description = "Fresh air."
      };
      Amenity mappedSubject = mapObject.Map(subject);
      Assert.Equal(temp, mappedSubject.AmenityId);
      Assert.Equal("Balcony", mappedSubject.AmenityType);
      Assert.Equal("Fresh air.", mappedSubject.Description);
    }
    [Fact]
    public void MapRoomTypeToEntity()
    {
      RoomType subject = new RoomType()
      {
        RoomTypeId = 4,
        Type = "Motel"
      };
      DataAccess.Entities.RoomType mappedSubject = mapObject.Map(subject);
      Assert.Equal(4, mappedSubject.RoomTypeId);
      Assert.Equal(4, subject.RoomTypeId);
    }
    [Fact]
    public void MapRoomTypeToLib()
    {
      DataAccess.Entities.RoomType subject = new DataAccess.Entities.RoomType()
      {
        RoomTypeId = 3,
        Type = "Hotel"
      };
      RoomType mappedSubject = mapObject.Map(subject);
      Assert.Equal(3, mappedSubject.RoomTypeId);
      Assert.Equal(3, subject.RoomTypeId);
    }
  }
}
