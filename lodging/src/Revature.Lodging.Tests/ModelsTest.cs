using Revature.Lodging.Lib.Models;
using System;
using Xunit;

namespace Revature.Lodging.Tests
{
  public class ModelsTest
  {
    [Fact]
    public void AssignAmentiy()
    {
      Amenity subject = new Amenity();
      Guid aID = Guid.NewGuid();
      subject.AmenityId = aID;
      subject.AmenityType = "Pool";
      subject.Description = "Open all year.";
      Assert.Equal(aID,subject.AmenityId);
      Assert.Equal("Pool", subject.AmenityType);
      Assert.Equal("Open all year.", subject.Description);
    }
    [Fact]
    public void AssignAmentiyComplex()
    {
      AmenityComplex subject = new AmenityComplex();
      Guid acID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      subject.AmenityComplexId = acID;
      subject.AmenityId = aID;
      subject.ComplexId = cID;
      Assert.Equal(acID,subject.AmenityComplexId);
      Assert.Equal(aID, subject.AmenityId);
      Assert.Equal(cID, subject.ComplexId);
    }
    [Fact]
    public void AssignAmentiyFloorPlan()
    {
      AmenityFloorPlan subject = new AmenityFloorPlan();
      Guid afpID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid fpID = Guid.NewGuid();
      subject.AmenityFloorPlanId = afpID;
      subject.AmenityId = aID;
      subject.FloorPlanId = fpID;
      Assert.Equal(afpID,subject.AmenityFloorPlanId);
      Assert.Equal(aID, subject.AmenityId);
      Assert.Equal(fpID, subject.FloorPlanId);
    }
    [Fact]
    public void AmentiyAssignRoom()
    {
      AmenityRoom subject = new AmenityRoom();
      Guid arID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid rID = Guid.NewGuid();
      subject.AmenityRoomId = arID;
      subject.AmenityId = aID;
      subject.RoomId = rID;
      Assert.Equal(arID,subject.AmenityRoomId);
      Assert.Equal(aID, subject.AmenityId);
      Assert.Equal(rID, subject.RoomId);
    }
    [Fact]
    public void AssignRoomType()
    {
      RoomType subject = new RoomType();
      subject.RoomTypeId = 5;
      subject.Type = "Apartment";
      Assert.Equal(5, subject.RoomTypeId);
      Assert.Equal("Apartment", subject.Type);
    }
    [Fact]
    public void AssignComplex()
    {
      Complex subject = new Complex();
      Guid cID = Guid.NewGuid();
      Guid adID = Guid.NewGuid();
      Guid pID = Guid.NewGuid();
      subject.ComplexId = cID;
      subject.AddressId = adID;
      subject.ProviderId = pID;
      subject.ComplexName = "Liv+";
      subject.ContactNumber = "123-456-7890";
      Assert.Equal(cID,subject.ComplexId);
      Assert.Equal(adID, subject.AddressId);
      Assert.Equal(pID, subject.ProviderId);
      Assert.Equal("Liv+", subject.ComplexName);
      Assert.Equal("123-456-7890",subject.ContactNumber);
    }

    [Fact]
    public void AssignFloorPlan()
    {
      FloorPlan subject = new FloorPlan();
      Guid fpID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      subject.FloorPlanId = fpID;
      subject.ComplexId = cID;
      subject.NumberOfBeds = 3;
      subject.FloorPlanName = "3 bedroom apartment";
      subject.RoomTypeId = 7;
      Assert.Equal(fpID,subject.FloorPlanId);
      Assert.Equal(cID, subject.ComplexId);
      Assert.Equal(3, subject.NumberOfBeds);
      Assert.Equal("3 bedroom apartment", subject.FloorPlanName);
      Assert.Equal(7, subject.RoomTypeId);
    }

    [Fact]
    public void AssignRoom()
    {
      Room subject = new Room();
      Guid rID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      Gender tempGender = new Gender() { GenderId = 5, Type = "Male" };
      RoomType tempRoomTypeId = new RoomType() { RoomTypeId = 8, Type = "Dorm" };
      DateTime tempStart = new DateTime(2020, 02, 28);
      DateTime tempEnd = new DateTime(2020, 06, 03);

      subject.RoomId = rID;
      subject.ComplexId = cID;
      subject.FloorPlanId = 7;
      subject.RoomNumber = "3422";
      subject.NumberBeds = 7;
      subject.NumberOccupants = 4;
      subject.Gender = tempGender;
      subject.RoomTypeId = tempRoomTypeId;
      subject.SetLease(tempStart, tempEnd);

      Assert.Equal(rID, subject.RoomId);
      Assert.Equal(cID, subject.ComplexId);
      Assert.Equal(7, subject.FloorPlanId);
      Assert.Equal("3422", subject.RoomNumber);
      Assert.Equal(7, subject.NumberBeds);
      Assert.Equal(4, subject.NumberOccupants);
      Assert.Equal(tempGender, subject.Gender);
      Assert.Equal(tempRoomTypeId, subject.RoomTypeId);
      Assert.Equal(tempStart, subject.LeaseStart);
      Assert.Equal(tempEnd, subject.LeaseEnd);
    }

    [Fact]
    public void AssignGender()
    {
      Gender subject = new Gender();
      subject.GenderId = 2;
      subject.Type = "Female";
      Assert.Equal(2, subject.GenderId);
      Assert.Equal("Female", subject.Type);
    }
  }
}
