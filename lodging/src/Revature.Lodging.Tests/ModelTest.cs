using System;
using Xunit;
using Revature.Lodging.Lib.Models;
using Revature.Lodging.DataAccess;
using System.Collections.Generic;
using System.Text;

namespace Revature.Lodging.Tests
{
  public class ModelTest
  {
    Mapper mapObject = new Mapper();

    [Fact]
    public void AssignAmentiy()
    {
      Amenity subject = new Amenity();
      Guid temp = Guid.NewGuid();
      subject.AmenityId = temp;
      subject.AmenityType = "Pool";
      subject.Description = "Open all year.";
      Assert.Equal(temp,subject.AmenityId);
    }
    [Fact]
    public void AssignAmentiyComplex()
    {
      AmenityComplex subject = new AmenityComplex();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      Guid temp3 = Guid.NewGuid();
      subject.AmenityComplexId = temp;
      subject.AmentityId = temp2;
      subject.ComplexId = temp3;
      Assert.Equal(temp,subject.AmenityComplexId);
    }
    [Fact]
    public void AssignAmentiyFloorPlan()
    {
      AmenityFloorPlan subject = new AmenityFloorPlan();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      Guid temp3 = Guid.NewGuid();
      subject.AmenityFloorPlanId = temp;
      subject.AmenityId = temp2;
      subject.FloorPlanId = temp3;
      Assert.Equal(temp,subject.AmenityFloorPlanId);
    }
    [Fact]
    public void AmentiyAssignRoom()
    {
      AmenityRoom subject = new AmenityRoom();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      Guid temp3 = Guid.NewGuid();
      subject.AmenityRoomId = temp;
      subject.AmenityId = temp2;
      subject.RoomId = temp3;
      Assert.Equal(temp,subject.AmenityRoomId);
    }
    [Fact]
    public void AssignRoomType()
    {
      RoomType subject = new RoomType();
      subject.RoomTypeId = 5;
      subject.Type = "Apartment";
      Assert.Equal(5,subject.RoomTypeId);
    }
    [Fact]
    public void AssignComplex()
    {
      Complex subject = new Complex();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      Guid temp3 = Guid.NewGuid();
      subject.ComplexId = temp;
      subject.AddressId = temp2;
      subject.ProviderId = temp3;
      subject.ComplexName = "Liv+";
      subject.ComplexName = "123-456-7890";
      Assert.Equal(temp,subject.ComplexId);
    }

    [Fact]
    public void AssignFloorPlan()
    {
      FloorPlan subject = new FloorPlan();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      subject.FloorPlanId = temp;
      subject.ComplexId = temp2;
      subject.NumberOfBeds = 3;
      subject.FloorPlanName = "3 bedroom apartment";
      subject.RoomTypeId = 7;
      Assert.Equal(temp,subject.FloorPlanId);
    }

    [Fact]
    public void AssignRoom()
    {
      Room subject = new Room();
      Guid temp = Guid.NewGuid();
      Guid temp2 = Guid.NewGuid();
      subject.RoomId = temp;
      subject.ComplexId = temp2;
      subject.FloorPlanId = 7;
      subject.RoomNumber = "3422";
      subject.NumberBeds = 7;
      subject.NumberOccupants = 4;
      subject.Gender = new Gender() { GenderId = 5, Type = "Male" };
      subject.RoomTypeId = new RoomType() { RoomTypeId = 8, Type = "Dorm" };
      subject.SetLease(new DateTime(2020,02,28), new DateTime(2020,06,30));
      Assert.Equal(temp, subject.RoomId);
    }

    [Fact]
    public void AssignGender()
    {
      Gender subject = new Gender();
      subject.GenderId = 2;
      subject.Type = "Female";
      Assert.Equal(2, subject.GenderId);
    }
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
