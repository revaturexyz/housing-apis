using Revature.Lodging.Lib.Models;
using Revature.Lodging.DataAccess;
using System;
using Xunit;

namespace Revature.Lodging.Tests
{
  public class MapTests
  {
    Mapper mapObject = new Mapper();

    /// <summary>
    /// Tests that the Amenity library model can be mapped to Amenity data access entity.
    /// </summary>
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

    /// <summary>
    /// Tests that the Amenity data access entity can be mapped to the Amenity library model.
    /// </summary>
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

    /// <summary>
    /// Tests that the AmenityComplex library model can be mapped to AmenityComplex data access entity.
    /// </summary>
    [Fact]
    public void MapAmentiyComplexToEntity()
    {
      Guid acID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      AmenityComplex subject = new AmenityComplex()
      {
        AmenityComplexId = acID,
        AmenityId = aID,
        ComplexId = cID
      };
      DataAccess.Entities.AmenityComplex mappedSubject = mapObject.Map(subject);
      Assert.Equal(acID, mappedSubject.AmenityComplexId);
      Assert.Equal(aID, mappedSubject.AmenityId);
      Assert.Equal(cID, mappedSubject.ComplexId);
    }

    /// <summary>
    /// Tests that the AmenityComplex data access entity can be mapped to the AmenityComplex library model.
    /// </summary>
    [Fact]
    public void MapAmentiyComplexToLib()
    {
      Guid acID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      DataAccess.Entities.AmenityComplex subject = new DataAccess.Entities.AmenityComplex()
      {
        AmenityComplexId = acID,
        AmenityId = aID,
        ComplexId = cID
      };
      AmenityComplex mappedSubject = mapObject.Map(subject);
      Assert.Equal(acID, mappedSubject.AmenityComplexId);
      Assert.Equal(aID, mappedSubject.AmenityId);
      Assert.Equal(cID, mappedSubject.ComplexId);
    }
    /// <summary>
    /// Tests that the AmenityFloorPlan library model can be mapped to AmenityFloorPlan data access entity.
    /// </summary>
    [Fact]
    public void MapAmentiyFloorPlanToEntity()
    {
      Guid afpID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid fpID = Guid.NewGuid();
      AmenityFloorPlan subject = new AmenityFloorPlan()
      {
        AmenityFloorPlanId = afpID,
        AmenityId = aID,
        FloorPlanId = fpID
      };
      DataAccess.Entities.AmenityFloorPlan mappedSubject = mapObject.Map(subject);
      Assert.Equal(afpID, mappedSubject.AmenityFloorPlanID);
      Assert.Equal(aID, mappedSubject.AmenityID);
      Assert.Equal(fpID, mappedSubject.FloorPlanID);
    }

    /// <summary>
    /// Tests that the AmenityFloorPlan data access entity can be mapped to the AmenityFloorPlan library model.
    /// </summary>
    [Fact]
    public void MapAmentiyFloorPlanToLib()
    {
      Guid afpID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid fpID = Guid.NewGuid();
      DataAccess.Entities.AmenityFloorPlan subject = new DataAccess.Entities.AmenityFloorPlan()
      {
        AmenityFloorPlanID = afpID,
        AmenityID = aID,
        FloorPlanID = fpID
      };
      AmenityFloorPlan mappedSubject = mapObject.Map(subject);
      Assert.Equal(afpID, mappedSubject.AmenityFloorPlanId);
      Assert.Equal(aID, mappedSubject.AmenityId);
      Assert.Equal(fpID, mappedSubject.FloorPlanId);
    }

    /// <summary>
    /// Tests that the AmenityRoom library model can be mapped to AmenityRoom data access entity.
    /// </summary>
    [Fact]
    public void MapAmentiyRoomToEntity()
    {
      Guid arID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid rID = Guid.NewGuid();
      AmenityRoom subject = new AmenityRoom()
      {
        AmenityRoomId = arID,
        AmenityId = aID,
        RoomId = rID
      };
      DataAccess.Entities.AmenityRoom mappedSubject = mapObject.Map(subject);
      Assert.Equal(arID, mappedSubject.AmenityRoomId);
      Assert.Equal(aID, mappedSubject.AmenityId);
      Assert.Equal(rID, mappedSubject.RoomId);
    }

    /// <summary>
    /// Tests that the AmenityRoom data access entity can be mapped to the AmenityRoom library model.
    /// </summary>
    [Fact]
    public void MapAmentiyRoomToLib()
    {
      Guid arID = Guid.NewGuid();
      Guid aID = Guid.NewGuid();
      Guid rID = Guid.NewGuid();
      DataAccess.Entities.AmenityRoom subject = new DataAccess.Entities.AmenityRoom()
      {
        AmenityRoomId = arID,
        AmenityId = aID,
        RoomId = rID
      };
      AmenityRoom mappedSubject = mapObject.Map(subject);
      Assert.Equal(arID, mappedSubject.AmenityRoomId);
      Assert.Equal(aID, mappedSubject.AmenityId);
      Assert.Equal(rID, mappedSubject.RoomId);
    }

    /// <summary>
    /// Tests that the Complex data access entity can be mapped to the Complex library model.
    /// </summary>
    [Fact]
    public void MapComplexToLib()
    {
      Guid cID = Guid.NewGuid();
      Guid adID = Guid.NewGuid();
      Guid pID = Guid.NewGuid();
      DataAccess.Entities.Complex subject = new DataAccess.Entities.Complex()
      {
        ComplexId = cID,
        AddressId = adID,
        ProviderId = pID,
        ComplexName = "Mariott",
        ContactNumber = "555-555-5555"
      };
      Complex mappedSubject = mapObject.Map(subject);
      Assert.Equal(cID, mappedSubject.ComplexId);
      Assert.Equal(adID, mappedSubject.AddressId);
      Assert.Equal(pID, mappedSubject.ProviderId);
      Assert.Equal("Mariott", mappedSubject.ComplexName);
      Assert.Equal("555-555-5555", mappedSubject.ContactNumber);
    }

    /// <summary>
    /// Tests that the Complex library model can be mapped to Complex data access entity.
    /// </summary>
    [Fact]
    public void MapComplexToEntity()
    {
      Guid cID = Guid.NewGuid();
      Guid adID = Guid.NewGuid();
      Guid pID = Guid.NewGuid();
      Complex subject = new Complex()
      {
        ComplexId = cID,
        AddressId = adID,
        ProviderId = pID,
        ComplexName = "Hilton",
        ContactNumber = "098-765-4321"
      };
      DataAccess.Entities.Complex mappedSubject = mapObject.Map(subject);
      Assert.Equal(cID, mappedSubject.ComplexId);
      Assert.Equal(adID, mappedSubject.AddressId);
      Assert.Equal(pID, mappedSubject.ProviderId);
      Assert.Equal("Hilton", mappedSubject.ComplexName);
      Assert.Equal("098-765-4321", mappedSubject.ContactNumber);
    }

    /// <summary>
    /// Tests that the Complex data access entity can be mapped to the Complex library model.
    /// </summary>
    [Fact]
    public void MapRoomToLib()
    {
      Guid rID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      Gender tempGender = new Gender() { GenderId = 2, Type = "Female" };
      RoomType tempRoomTypeId = new RoomType() { RoomTypeId = 9, Type = "Air BNB" };
      DateTime tempStart = new DateTime(2019, 12, 15);
      DateTime tempEnd = new DateTime(2019, 12, 29);
      DataAccess.Entities.Room subject = new DataAccess.Entities.Room()
      {
        RoomId = rID,
        ComplexId = cID,
        FloorPlanID = 6,
        RoomNumber = "1221",
        NumberOfBeds = 4,
        NumberOfOccupants = 3,
        Gender = mapObject.Map(tempGender),
        RoomType = mapObject.Map(tempRoomTypeId),
        LeaseStart = tempStart,
        LeaseEnd = tempEnd
      };
      Room mappedSubject = mapObject.Map(subject);
      Assert.Equal(rID, mappedSubject.RoomId);
      Assert.Equal(cID, mappedSubject.ComplexId);
      Assert.Equal(6, mappedSubject.FloorPlanId);
      Assert.Equal("1221", mappedSubject.RoomNumber);
      Assert.Equal(4, mappedSubject.NumberBeds);
      Assert.Equal(3, mappedSubject.NumberOccupants);
      Assert.Equal(tempGender.GenderId, mappedSubject.Gender.GenderId);
      Assert.Equal(tempGender.Type, mappedSubject.Gender.Type);
      Assert.Equal(tempRoomTypeId.RoomTypeId, mappedSubject.RoomTypeId.RoomTypeId);
      Assert.Equal(tempRoomTypeId.Type, mappedSubject.RoomTypeId.Type);
      Assert.Equal(tempStart, mappedSubject.LeaseStart);
      Assert.Equal(tempEnd, mappedSubject.LeaseEnd);
    }

    /// <summary>
    /// Tests that the Complex library model can be mapped to Complex data access entity.
    /// </summary>
    [Fact]
    public void MapRoomToEntity()
    {
      Guid rID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      Gender tempGender = new Gender() { GenderId = 2, Type = "Female" };
      DataAccess.Entities.Gender tempGender2 = mapObject.Map(tempGender);
      RoomType tempRoomTypeId = new RoomType() { RoomTypeId = 9, Type = "Air BNB" };
      DataAccess.Entities.RoomType tempRoomTypeId2 = mapObject.Map(tempRoomTypeId);
   
      DateTime tempStart = new DateTime(2019, 12, 15);
      DateTime tempEnd = new DateTime(2019, 12, 29);
      Room subject = new Room()
      {
        RoomId = rID,
        ComplexId = cID,
        FloorPlanId = 10,
        RoomNumber = "888",
        NumberBeds = 4,
        NumberOccupants = 4,
        Gender = tempGender,
        RoomTypeId = tempRoomTypeId
      };
      subject.SetLease(tempStart, tempEnd);

      DataAccess.Entities.Room mappedSubject = mapObject.Map(subject);
      Assert.Equal(rID, mappedSubject.RoomId);
      Assert.Equal(cID, mappedSubject.ComplexId);
      Assert.Equal(10, mappedSubject.FloorPlanID);
      Assert.Equal("888", mappedSubject.RoomNumber);
      Assert.Equal(4, mappedSubject.NumberOfBeds);
      Assert.Equal(4, mappedSubject.NumberOfOccupants);
      Assert.Equal(tempGender2.GenderID, mappedSubject.Gender.GenderID);
      Assert.Equal(tempGender2.Type, mappedSubject.Gender.Type);
      Assert.Equal(tempRoomTypeId2.RoomTypeId, mappedSubject.RoomType.RoomTypeId);
      Assert.Equal(tempRoomTypeId2.Type, mappedSubject.RoomType.Type);
      Assert.Equal(tempStart, mappedSubject.LeaseStart);
      Assert.Equal(tempEnd, mappedSubject.LeaseEnd);
    }

    /// <summary>
    /// Tests that the FloorPlan data access entity can be mapped to the FloorPlan library model.
    /// </summary>
    [Fact]
    public void MapFloorPlanToLib()
    {
      Guid fpID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      DataAccess.Entities.FloorPlan subject = new DataAccess.Entities.FloorPlan()
      {
        FloorPlanID = fpID,
        ComplexID = cID,
        FloorPlanName = "2 rooms and balcony",
        NumberBeds = 4,
        RoomTypeID = 3
      };
      FloorPlan mappedSubject = mapObject.Map(subject);
      Assert.Equal(fpID, mappedSubject.FloorPlanId);
      Assert.Equal(cID, mappedSubject.ComplexId);
      Assert.Equal("2 rooms and balcony", mappedSubject.FloorPlanName);
      Assert.Equal(4, mappedSubject.NumberOfBeds);
      Assert.Equal(3, mappedSubject.RoomTypeId);
    }

    /// <summary>
    /// Tests that the Floor Plan library model can be mapped to the FloorPlan data access entity.
    /// </summary>
    [Fact]
    public void MapFloorPlanToEntity()
    {
      Guid fpID = Guid.NewGuid();
      Guid cID = Guid.NewGuid();
      FloorPlan subject = new FloorPlan()
      {
        FloorPlanId = fpID,
        ComplexId = cID,
        FloorPlanName = "5 rooms with 10 beds",
        NumberOfBeds = 10,
        RoomTypeId = 8
      };
      DataAccess.Entities.FloorPlan mappedSubject = mapObject.Map(subject);
      Assert.Equal(fpID, mappedSubject.FloorPlanID);
      Assert.Equal(cID, mappedSubject.ComplexID);
      Assert.Equal("5 rooms with 10 beds", mappedSubject.FloorPlanName);
      Assert.Equal(10, mappedSubject.NumberBeds);
      Assert.Equal(8, mappedSubject.RoomTypeID);
    }
   
    /// <summary>
    /// Tests that the RoomType Library model can be mapped to the RoomType data access entity.
    /// </summary>
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
      Assert.Equal("Motel", mappedSubject.Type);
    }

    /// <summary>
    /// Tests that the RoomType data access Entity can be mapped to the Roomtype library model.
    /// </summary>
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
      Assert.Equal("Hotel", mappedSubject.Type);
    }

    /// <summary>
    /// Tests that the Gender Library model can be mapped to the Gender data access entity.
    /// </summary>
    [Fact]
    public void MapGenderToEntity()
    {
      Gender subject = new Gender()
      {
        GenderId = 1,
        Type = "Male"
      };
      DataAccess.Entities.Gender mappedSubject = mapObject.Map(subject);
      Assert.Equal(1, mappedSubject.GenderID);
      Assert.Equal("Male", mappedSubject.Type);
    }

    /// <summary>
    /// Tests that the Gender data access Entity can be mapped to the Gender library model.
    /// </summary>
    [Fact]
    public void MapGenderToLib()
    {
      DataAccess.Entities.Gender subject = new DataAccess.Entities.Gender()
      {
        GenderID = 2,
        Type = "Female"
      };
      Gender mappedSubject = mapObject.Map(subject);
      Assert.Equal(2, mappedSubject.GenderId);
      Assert.Equal("Female", mappedSubject.Type);
    }
  }
}
