using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Revature.Lodging.DataAccess;
using Revature.Lodging.DataAccess.Repository;
using Xunit;
using Entity = Revature.Lodging.DataAccess.Entities;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Tests.DataTests
{
  public class RepositoryTest
  {
    // complex IDs
    private static readonly Guid s_cId1 = Guid.NewGuid();
    private static readonly Guid s_cId2 = Guid.NewGuid();

    // address IDs
    private static readonly Guid s_aId1 = Guid.NewGuid();
    private static readonly Guid s_aId2 = Guid.NewGuid();

    // provider IDs
    private static readonly Guid s_pId1 = Guid.NewGuid();
    private static readonly Guid s_pId2 = Guid.NewGuid();

    // room IDs
    private static readonly Guid s_rId = Guid.NewGuid();

    // amenity IDs
    private static readonly Guid s_amId1 = Guid.NewGuid();
    private static readonly Guid s_amId2 = Guid.NewGuid();

    // amenity-complex IDs
    private static readonly Guid s_acId1 = Guid.NewGuid();
    private static readonly Guid s_acId2 = Guid.NewGuid();

    // amenity-room IDs
    private static readonly Guid s_arId1 = Guid.NewGuid();
    private static readonly Guid s_arId2 = Guid.NewGuid();

    private readonly Logic.Complex _complex1 = new Logic.Complex
    {
      Id = s_cId1,
      AddressId = s_aId1,
      ProviderId = s_pId1,
      ComplexName = "Liv+",
      ContactNumber = "1234567890"
    };

    private readonly Logic.Complex _complex2 = new Logic.Complex
    {
      Id = s_cId2,
      AddressId = s_aId2,
      ProviderId = s_pId2,
      ComplexName = "SampleComplex",
      ContactNumber = "9876543210"
    };

    private readonly Entity.Complex _complexE1 = new Entity.Complex
    {
      Id = s_cId1,
      AddressId = s_aId1,
      ProviderId = s_pId1,
      ComplexName = "Liv+",
      ContactNumber = "1234567890"
    };

    private readonly Entity.Complex _complexE2 = new Entity.Complex
    {
      Id = s_cId2,
      AddressId = s_aId2,
      ProviderId = s_pId2,
      ComplexName = "SampleComplex",
      ContactNumber = "9876543210"
    };

    private readonly Logic.RoomAmenity _ar = new Logic.RoomAmenity
    {
      Id = s_arId1,
      RoomId = s_rId,
      AmenityId = s_amId1
    };

    private readonly Logic.ComplexAmenity _ac = new Logic.ComplexAmenity
    {
      Id = s_acId1,
      ComplexId = s_cId1,
      AmenityId = s_amId1
    };

    private readonly Entity.ComplexAmenity _acE1 = new Entity.ComplexAmenity
    {
      Id = s_acId1,
      ComplexId = s_cId1,
      AmenityId = s_amId1
    };

    private readonly Entity.ComplexAmenity _acE2 = new Entity.ComplexAmenity
    {
      Id = s_acId2,
      ComplexId = s_cId1,
      AmenityId = s_amId2
    };

    private readonly Logic.Amenity _amenity = new Logic.Amenity
    {
      Id = s_amId1,
      AmenityType = "Fridge",
      Description = "frozen"
    };

    private readonly Entity.Amenity _am1 = new Entity.Amenity
    {
      Id = s_amId1,
      AmenityType = "Fridge",
      Description = "to freeze"
    };

    private readonly Entity.Amenity _am2 = new Entity.Amenity
    {
      Id = s_amId2,
      AmenityType = "Test2",
      Description = "to heat"
    };

    private readonly Entity.RoomAmenity _arE1 = new Entity.RoomAmenity
    {
      Id = s_arId1,
      RoomId = s_rId,
      AmenityId = s_amId1
    };

    private readonly Entity.RoomAmenity _arE2 = new Entity.RoomAmenity
    {
      Id = s_arId2,
      RoomId = s_rId,
      AmenityId = s_amId2
    };

    /// <summary>
    /// This test is to test CreateComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void CreateComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("CreateComplexAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      var result = await complexRepo.CreateComplexAsync(_complex1);
      var checker = testContext.Complex.First().Id;

      Assert.Equal(checker, s_cId1);
    }

    /// <summary>
    /// This test is to test ReadComplexListAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadComplexListAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadComplexListTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);
      testContext.SaveChanges();

      var list = await complexRepo.ReadComplexListAsync();

      Assert.Equal("Liv+", list[0].ComplexName);
      Assert.Equal("9876543210", list[1].ContactNumber);
    }

    /// <summary>
    /// This test is to test ReadComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadComplexAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      testContext.Add(_complexE1);

      var read = await complexRepo.ReadComplexByIdAsync(s_cId1);

      Assert.Equal(s_cId1, read.Id);
    }

    /// <summary>
    /// This test is to test UpdateComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void UpdateComplexAsync()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("UpdateComplexAsync")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      testContext.Add(_complexE1);
      testContext.SaveChanges();

      var update = new Logic.Complex
      {
        Id = s_cId1,
        ComplexName = "Liv++",
        ContactNumber = "7894561231"
      };
      _ = await complexRepo.UpdateComplexAsync(update);
      var result = testContext.Complex.Find(s_cId1).ComplexName;
      var phone = testContext.Complex.Find(s_cId1).ContactNumber;

      Assert.Equal("Liv++", result);
      Assert.Equal("7894561231", phone);
    }

    /// <summary>
    /// This test is to test DeleteComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void DeleteComplexAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("DeleteComplexTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);

      var status = await complexRepo.DeleteComplexAsync(s_cId1);

      var result = testContext.Complex.First().ComplexName;
      var phone = testContext.Complex.First().ContactNumber;

      Assert.Equal("SampleComplex", result);
      Assert.Equal("9876543210", phone);
    }

    /// <summary>
    /// This test is to test CreateAmenityRoomAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void CreateAmenityRoomAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("CreateAmenityRoomAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);

      var repo = new AmenityRepository(testContext, log);

      var result = await repo.CreateAmenityRoomAsync(_ar);
      var check = testContext.RoomAmenity.First().Id;

      Assert.Equal(check, _ar.Id);
    }

    /// <summary>
    /// This test is to test CreateAmenityComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void CreateAmenityComplexAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("CreateAmenityComplexAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      var result = await repo.CreateAmenityComplexAsync(_ac);
      var check = testContext.ComplexAmenity.First().Id;

      Assert.Equal(check, _ac.Id);
    }

    /// <summary>
    /// This test is to test CreateAmenityAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void CreateAmenityAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("CreateAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      var result = await repo.CreateAmenityAsync(_amenity);

      var check = testContext.Amenity.First().Id;

      Assert.Equal(_amenity.Id, check);
    }

    /// <summary>
    /// This test is to test ReadAmenityList in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadAmenityListAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadAmenityListTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListAsync();

      Assert.Equal(_am1.Id, am[0].Id);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadAmenityListByComplexId in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadAmenityListByComplexIdAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadAmenityListByComplexIdTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_complexE1);
      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.Add(_acE1);
      testContext.Add(_acE2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListByComplexIdAsync(s_cId1);

      Assert.Equal("Fridge", am[0].AmenityType);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadAmenityListByRoomId in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadAmenityListByRoomIdAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadAmenityListByRoomIdTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);
      testContext.Add(_arE1);
      testContext.Add(_arE2);
      testContext.SaveChanges();

      var am = await repo.ReadAmenityListByRoomIdAsync(s_rId);

      Assert.Equal("Fridge", am[0].AmenityType);
      Assert.Equal("Test2", am[1].AmenityType);
    }

    /// <summary>
    /// This test is to test ReadComplexByProviderAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadComplexByProviderIDAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("ReadComplexByProviderIDTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      var complexE3 = new Entity.Complex
      {
        Id = Guid.NewGuid(),
        AddressId = Guid.NewGuid(),
        ProviderId = s_pId1,
        ComplexName = "XXX",
        ContactNumber = "1234567895"
      };

      testContext.Add(_complexE1);
      testContext.Add(_complexE2);
      testContext.Add(complexE3);
      testContext.SaveChanges();

      var complices = await complexRepo.ReadComplexByProviderIdAsync(s_pId1);

      Assert.Equal("Liv+", complices[0].ComplexName);
      Assert.Equal("XXX", complices[1].ComplexName);
    }

    /// <summary>
    /// This test is to test UpdateAmenityAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void UpdateAmenityAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("UpdateAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_am1);
      testContext.Add(_am2);

      var update1 = new Logic.Amenity
      {
        Id = s_amId1,
        AmenityType = "Microwave",
        Description = "To heat foods"
      };

      await repo.UpdateAmenityAsync(update1);
      var check1 = testContext.Amenity.Find(s_amId1);

      Assert.Equal("Microwave", check1.AmenityType);
    }

    /// <summary>
    /// This test is to test DeleteAmenityAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void DeleteAmenityAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("DeleteAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

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
        Id = s_amId1
      };

      await repo.DeleteAmenityAsync(delete1);

      var check = testContext.Amenity.First();

      Assert.Equal(s_amId2, check.Id);
    }

    /// <summary>
    /// This test is to test ReadComplexByNameAndNumberAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void ReadComplexByNameAndNumberAsyncTest()
    {
      var log = new NullLogger<ComplexRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("DeleteAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var roomRepo = new RoomRepository(testContext);
      var complexRepo = new ComplexRepository(testContext, roomRepo, log);

      var name = "Liv+";
      var phone = "1234567890";

      testContext.Add(_complexE1);
      testContext.SaveChanges();

      var check = await complexRepo.ReadComplexByNameAndNumberAsync(name, phone);

      Assert.Equal(name, check.ComplexName);
      Assert.Equal(phone, check.ContactNumber);
    }

    /// <summary>
    /// This test is to test DeleteAmenityRoomAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void DeleteAmenityRoomAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("DeleteAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_arE1);
      testContext.Add(_arE2);
      testContext.SaveChanges();

      await repo.DeleteAmenityRoomAsync(s_rId);

      Assert.Null(testContext.RoomAmenity.Find(s_rId));
    }

    /// <summary>
    /// This test is to test DeleteAmenityComplexAsync in DataAccess.Repository.
    /// </summary>
    [Fact]
    public async void DeleteAmenityComplexAsyncTest()
    {
      var log = new NullLogger<AmenityRepository>();
      var options = new DbContextOptionsBuilder<Entity.LodgingDbContext>()
        .UseInMemoryDatabase("DeleteAmenityAsyncTest")
        .Options;
      using var testContext = new Entity.LodgingDbContext(options);
      var repo = new AmenityRepository(testContext, log);

      testContext.Add(_acE1);
      testContext.Add(_acE2);
      testContext.SaveChanges();

      await repo.DeleteAmenityRoomAsync(s_cId1);

      Assert.Null(testContext.ComplexAmenity.Find(s_cId1));
    }
  }
}
