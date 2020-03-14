using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Revature.Address.Api.Controllers;
using Revature.Address.DataAccess;
using Revature.Address.Lib.BusinessLogic;
using Revature.Address.Tests.DataAccess.Tests;
using Xunit;

namespace Revature.Address.Tests.Api.Tests
{
  public class ControllerTests
  {
    /// <summary>
    /// Tests that Constructor for Tenant Controller successfully constructs
    /// </summary>
    [Fact]
    public void ConstructorShouldConstruct()
    {
      // arrange (create database)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      var options = TestDbContext.TestDbInitalizer("CanCreate");
      using var database = TestDbContext.CreateTestDb(options);
      var mapper = new Mapper();
      var logger = new NullLogger<Address.DataAccess.DataAccess>();
      var dataAccess = new Address.DataAccess.DataAccess(database, mapper, logger);

      // act (pass repository with database into controller)
      var test = new AddressController(dataAccess, mockAddressLogic.Object, mockLogger.Object);

      // assert (test passes if no exception thrown)
    }



    [Fact]
    public async void CheckDistanceShouldCheckDistance()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      var options = TestDbContext.TestDbInitalizer("CanCreate");
      using var database = TestDbContext.CreateTestDb(options);
      var mapper = new Mapper();
      var logger = new NullLogger<Address.DataAccess.DataAccess>();
      var dataAccess = new Address.DataAccess.DataAccess(database, mapper, logger);
      database.Add(AddressTestData.ValidAddress1());
      var test = new AddressController(dataAccess, mockAddressLogic.Object, mockLogger.Object);
      var validAddress1 = new Address.Api.Models.AddressModel
      {
        Id = new Guid("566e1a61-c283-4d33-9b9b-9a981393cf2b"),
        Street = "1100 S W St",
        City = "Arlington",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      var validAddress2 = new Address.Api.Models.AddressModel
      {
        Id = new Guid("2d9fe0e6-8029-483d-b5e9-c06cf2fad0d6"),
        Street = "1560 Broadway Street",
        City = "Boulder",
        State = "Colorado",
        Country = "US",
        ZipCode = "80112"
      };

      // act (get check distance between addresses)
      var result = await test.IsInRange(validAddress1, validAddress2);

      // Assert correct address was receive

      Assert.False(result.Value);
    }

    [Fact]
    public async void GetAddressShouldReturnNull()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      var options = TestDbContext.TestDbInitalizer("CanCreate");
      using var database = TestDbContext.CreateTestDb(options);
      var mapper = new Mapper();
      var logger = new NullLogger<Address.DataAccess.DataAccess>();
      var dataAccess = new Address.DataAccess.DataAccess(database, mapper, logger);
      var test = new AddressController(dataAccess, mockAddressLogic.Object, mockLogger.Object);
      var newAddy = new Address.Api.Models.AddressModel
      {
        Id = new Guid("566e1a61-c283-4d33-9b9b-9a981393cf2b"),
        Street = "ooooooo",
        City = "oooooo",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      // act (send address to database)
      var address = await test.GetAddress(newAddy);

      // Assert correct address was received
      Assert.Null(address.Value);
    }
  }
}
