using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Revature.Lodging.DataAccess;
using Revature.Lodging.DataAccess.Repository;
using System;
using System.Linq;
using Xunit;
using Entity = Revature.Lodging.DataAccess.Entities;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Tests.DataTests
{
  public class RepositoryTest
  {
    //private readonly NullLogger<Repository> log;

    # region data to test 
    public static Guid CId1 = Guid.NewGuid();
    public static Guid AId1 = Guid.NewGuid();
    public static Guid PId1 = Guid.NewGuid();
    public static Guid CId2 = Guid.NewGuid();
    public static Guid AId2 = Guid.NewGuid();
    public static Guid PId2 = Guid.NewGuid();
    public static Guid RId = Guid.NewGuid();
    public static Guid AmId1 = Guid.NewGuid();
    public static Guid AmId2 = Guid.NewGuid();
    public static Guid AcId1 = Guid.NewGuid();
    public static Guid AcId2 = Guid.NewGuid();
    public static Guid ArId1 = Guid.NewGuid();
    public static Guid ArId2 = Guid.NewGuid();

    public Logic.Complex _complex1 = new Logic.Complex
    {
      Id = CId1,
      AddressId = AId1,
      ProviderId = PId1,
      ComplexName = "Liv+",
      ContactNumber = "1234567890"
    };

    public Logic.Complex _complex2 = new Logic.Complex
    {
      Id = CId2,
      AddressId = AId2,
      ProviderId = PId2,
      ComplexName = "SampleComplex",
      ContactNumber = "9876543210"
    };

    public Entity.Complex _complexE1 = new Entity.Complex
    {
      Id = CId1,
      AddressId = AId1,
      ProviderId = PId1,
      ComplexName = "Liv+",
      ContactNumber = "1234567890"
    };

    public Entity.Complex _complexE2 = new Entity.Complex
    {
      Id = CId2,
      AddressId = AId2,
      ProviderId = PId2,
      ComplexName = "SampleComplex",
      ContactNumber = "9876543210"
    };

    public Logic.RoomAmenity _ar = new Logic.RoomAmenity
    {
      Id = ArId1,
      RoomId = RId,
      AmenityId = AmId1
    };

    public Logic.ComplexAmenity _ac = new Logic.ComplexAmenity
    {
      Id = AcId1,
      ComplexId = CId1,
      AmenityId = AmId1
    };

    public Entity.ComplexAmenity _acE1 = new Entity.ComplexAmenity
    {
      Id = AcId1,
      ComplexId = CId1,
      AmenityId = AmId1
    };

    public Entity.ComplexAmenity _acE2 = new Entity.ComplexAmenity
    {
      Id = AcId2,
      ComplexId = CId1,
      AmenityId = AmId2
    };

    public Logic.Amenity _amenity = new Logic.Amenity
    {
      Id = AmId1,
      AmenityType = "Fridge",
      Description = "frozen"
    };

    public Entity.Amenity _am1 = new Entity.Amenity
    {
      Id = AmId1,
      AmenityType = "Fridge",
      Description = "to freeze"
    };

    public Entity.Amenity _am2 = new Entity.Amenity
    {
      Id = AmId2,
      AmenityType = "Test2",
      Description = "to heat"
    };

    public Entity.RoomAmenity _arE1 = new Entity.RoomAmenity
    {
      Id = ArId1,
      RoomId = RId,
      AmenityId = AmId1
    };

    public Entity.RoomAmenity _arE2 = new Entity.RoomAmenity
    {
      Id = ArId2,
      RoomId = RId,
      AmenityId = AmId2
    };
    #endregion 

    /// <summary>
    /// This test is to test CreateComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void CreateComplexAsyncTest()
    {

      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("CreateComplexAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var result = await repo.CreateComplexAsync(_complex1);
      var checker = testContext.Complex.First().Id;

      Assert.Equal(checker, CId1);
    }

    /// <summary>
    /// This test is to test ReadComplexListAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadComplexListAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadComplexListTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);
      testContext.SaveChanges();

      var list = await repo.ReadComplexListAsync();

      Assert.Equal("Liv+", list[0].ComplexName);
      Assert.Equal("9876543210", list[1].ContactNumber);
    }

    /// <summary>
    /// This test is to test ReadComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadComplexAsyncTest()
    {

      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadComplexAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_complexE1);

      var read = await repo.ReadComplexByIdAsync(CId1);

      Assert.Equal(CId1, read.Id);
    }

    /// <summary>
    /// This test is to test UpdateComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void UpdateComplexAsync()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("UpdateComplexAsync")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_complexE1);
      testContext.SaveChanges();

      var update = new Logic.Complex
      {
        Id = CId1,
        ComplexName = "Liv++",
        ContactNumber = "7894561231"
      };
      _ = await repo.UpdateComplexAsync(update);
      var result = testContext.Complex.Find(CId1).ComplexName;
      var phone = testContext.Complex.Find(CId1).ContactNumber;

      Assert.Equal("Liv++", result);
      Assert.Equal("7894561231", phone);
    }

    /// <summary>
    /// This test is to test DeleteComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void DeleteComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("DeleteComplexTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);

      var status = await repo.DeleteComplexAsync(CId1);

      var result = testContext.Complex.First().ComplexName;
      var phone = testContext.Complex.First().ContactNumber;

      Assert.Equal("SampleComplex", result);
      Assert.Equal("9876543210", phone);
    }

    /// <summary>
    /// This test is to test CreateAmenityRoomAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void CreateAmenityRoomAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("CreateAmenityRoomAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var result = await repo.CreateAmenityRoomAsync(_ar);
      var check = testContext.AmenityRoom.First().Id;

      Assert.Equal(check, _ar.Id);
    }

    /// <summary>
    /// This test is to test CreateAmenityComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void CreateAmenityComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("CreateAmenityComplexAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var result = await repo.CreateAmenityComplexAsync(_ac);
      var check = testContext.AmenityComplex.First().Id;

      Assert.Equal(check, _ac.Id);
    }

    /// <summary>
    /// This test is to test CreateAmenityAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void CreateAmenityAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("CreateAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var result = await repo.CreateAmenityAsync(_amenity);

      var check = testContext.Amenity.First().Id;

      Assert.Equal(_amenity.Id, check);
    }

    /// <summary>
    /// This test is to test ReadAmenityList in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadAmenityListAsyncTest()
    {

      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadAmenityListTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListAsync();

      Assert.Equal(_am1.Id, am[0].Id);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadAmenityListByComplexId in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadAmenityListByComplexIdAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadAmenityListByComplexIdTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_complexE1);
      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.Add(_acE1);
      testContext.Add(_acE2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListByComplexIdAsync(CId1);

      Assert.Equal("Fridge", am[0].AmenityType);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadAmenityListByRoomId in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadAmenityListByRoomIdAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadAmenityListByRoomIdTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.Add(_arE1);
      testContext.Add(_arE2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListByRoomIdAsync(RId);

      Assert.Equal("Fridge", am[0].AmenityType);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadComplexByProviderAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadComplexByProviderIDAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("ReadComplexByProviderIDTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var complexE3 = new Entity.Complex
      {
        Id = Guid.NewGuid(),
        AddressId = Guid.NewGuid(),
        ProviderId = PId1,
        ComplexName = "XXX",
        ContactNumber = "1234567895"
      };

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);
      testContext.Add(complexE3);
      testContext.SaveChanges();

      var complices = await repo.ReadComplexByProviderIdAsync(PId1);

      Assert.Equal("Liv+", complices[0].ComplexName);
      Assert.Equal("XXX", complices[1].ComplexName);
    }

    /// <summary>
    /// This test is to test UpdateAmenityAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void UpdateAmenityAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("UpdateAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);

      var update1 = new Logic.Amenity
      {
        Id = AmId1,
        AmenityType = "Microwave",
        Description = "To heat foods"
      };

      await repo.UpdateAmenityAsync(update1);
      var check1 = testContext.Amenity.Find(AmId1);

      Assert.Equal("Microwave", check1.AmenityType);
    }

    /// <summary>
    /// This test is to test DeleteAmenityAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void DeleteAmenityAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("DeleteAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var amId3 = Guid.NewGuid();
      var am3 = new Entity.Amenity
      {
        Id = amId3,
        AmenityType = "Pool",
        Description = "swimming"
      };

      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.Add(am3);

      var delete1 = new Logic.Amenity
      {
        Id = AmId1
      };

      await repo.DeleteAmenityAsync(delete1);

      var check = testContext.Amenity.First();

      Assert.Equal(AmId2, check.Id);
    }

    /// <summary>
    /// This test is to test ReadComplexByNameAndNumberAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void ReadComplexByNameAndNumberAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("DeleteAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      var name = "Liv+";
      var phone = "1234567890";

      testContext.Add(_complexE1);
      testContext.SaveChanges();

      var check = await repo.ReadComplexByNameAndNumberAsync(name, phone);

      Assert.Equal(name, check.ComplexName);
      Assert.Equal(phone, check.ContactNumber);
    }

    /// <summary>
    /// This test is to test DeleteAmenityRoomAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void DeleteAmenityRoomAsyncTest()
    {

      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("DeleteAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_arE1);
      testContext.Add(_arE2);
      testContext.SaveChanges();

      await repo.DeleteAmenityRoomAsync(RId);

      Assert.Null(testContext.AmenityRoom.Find(RId));
    }

    /// <summary>
    /// This test is to test DeleteAmenityComplexAsync in DataAccess.Repository
    /// </summary>
    [Fact]
    public async void DeleteAmenityComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options
          = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
              .UseInMemoryDatabase("DeleteAmenityAsyncTest")
              .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new ComplexRepository(testContext, log);

      testContext.Add(_acE1);
      testContext.Add(_acE2);
      testContext.SaveChanges();

      await repo.DeleteAmenityRoomAsync(CId1);

      Assert.Null(testContext.AmenityComplex.Find(CId1));
    }


  }
}
