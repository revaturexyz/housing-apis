using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    public async void CheckDistanceShouldReturnFalseIfDistanceIsOutOfRange()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      mockAddressLogic.Setup(mal => mal.IsInRangeAsync(It.IsAny<Address.Lib.Address>(), It.IsAny<Address.Lib.Address>(), It.IsAny<int>())).Returns(async () =>
      {
        await Task.Yield();
        return false;
      });
      var mockDataAccess = new Mock<Address.DataAccess.Interfaces.IDataAccess>();
      var test = new AddressController(mockDataAccess.Object, mockAddressLogic.Object, mockLogger.Object);
      var validAddress1 = new Address.Api.Models.AddressModel
      {
        Street = "1100 S W St",
        City = "Arlington",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      var validAddress2 = new Address.Api.Models.AddressModel
      {
        Street = "1560 Broadway Street",
        City = "Boulder",
        State = "Colorado",
        Country = "US",
        ZipCode = "80112"
      };
      var list = new List<Address.Api.Models.AddressModel>
      {
        validAddress1,
        validAddress2
      };

      // act (get check distance between addresses)
      var result = await test.IsInRange(list);

      // Assert correct address was receive

      Assert.False(result.Value);
    }

    [Fact]
    public async void CheckDistanceShouldReturnTrueIfDistanceIsInRange()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      mockAddressLogic.Setup(mal => mal.IsInRangeAsync(It.IsAny<Address.Lib.Address>(), It.IsAny<Address.Lib.Address>(), It.IsAny<int>())).Returns(async () =>
      {
        await Task.Yield();
        return true;
      });
      var mockDataAccess = new Mock<Address.DataAccess.Interfaces.IDataAccess>();
      var test = new AddressController(mockDataAccess.Object, mockAddressLogic.Object, mockLogger.Object);
      var validAddress1 = new Address.Api.Models.AddressModel
      {
        Street = "1100 S W St",
        City = "Arlington",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      var validAddress2 = new Address.Api.Models.AddressModel
      {
        Street = "1560 Broadway Street",
        City = "Boulder",
        State = "Colorado",
        Country = "US",
        ZipCode = "80112"
      };
      var list = new List<Address.Api.Models.AddressModel>
      {
        validAddress1,
        validAddress2
      };

      // act (get check distance between addresses)
      var result = await test.IsInRange(list);

      // Assert correct address was receive

      Assert.True(result.Value);
    }

    [Fact]
    public async void PostBadAddressShouldReturn400()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      mockAddressLogic.Setup(al => al.IsValidAddressAsync(It.IsAny<Address.Lib.Address>())).Returns(async () => {
        await Task.Yield();
        return false;
      });
      var options = TestDbContext.TestDbInitalizer("BadAddress400");
      using var database = TestDbContext.CreateTestDb(options);
      var mapper = new Mapper();
      var logger = new NullLogger<Address.DataAccess.DataAccess>();
      var dataAccess = new Address.DataAccess.DataAccess(database, mapper, logger);
      var test = new AddressController(dataAccess, mockAddressLogic.Object, mockLogger.Object);
      var newAddy = new Address.Api.Models.AddressModel
      {
        Street = "ooooooo",
        City = "oooooo",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      // act (send address to database)
      ActionResult<Address.Api.Models.AddressModel> address = await test.PostAddress(newAddy);

      // Assert correct address was received

      Assert.Equal(400, ((IStatusCodeActionResult)address.Result).StatusCode);
    }

    [Fact]
    public async void PostGoodAddressShouldReturn201()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      mockAddressLogic.Setup(al => al.IsValidAddressAsync(It.IsAny<Address.Lib.Address>())).Returns(async () => {
        await Task.Yield();
        return true;
      });
      mockAddressLogic.Setup(al => al.NormalizeAddressAsync(It.IsAny<Address.Lib.Address>())).Returns<Address.Lib.Address>(async (address) => {
        await Task.Yield();
        return address;
      });
      var options = TestDbContext.TestDbInitalizer("GoodAddress201");
      using var database = TestDbContext.CreateTestDb(options);
      var mapper = new Mapper();
      var logger = new NullLogger<Address.DataAccess.DataAccess>();
      var dataAccess = new Address.DataAccess.DataAccess(database, mapper, logger);
      var test = new AddressController(dataAccess, mockAddressLogic.Object, mockLogger.Object);
      var newAddy = new Address.Api.Models.AddressModel
      {
        Street = "ooooooo",
        City = "oooooo",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      // act (send address to database)
      ActionResult<Address.Api.Models.AddressModel> address = await test.PostAddress(newAddy);

      // Assert correct address was received

      Assert.Equal(201, ((IStatusCodeActionResult)address.Result).StatusCode);
    }

    [Fact]
    public async void PostConflictingAddressShouldReturn4()
    {
      // Arrange (create a moq repo and add a test address)
      var mockLogger = new Mock<ILogger<AddressController>>();
      var mockAddressLogic = new Mock<IAddressLogic>();
      var mockDataAccess = new Mock<Address.DataAccess.Interfaces.IDataAccess>();
      mockDataAccess.Setup(da => da.GetAddressAsync(null, It.IsAny<Address.Lib.Address>())).Returns<Guid,Address.Lib.Address>(async (id,address) =>
       {
         await Task.Yield();
         return new List<Address.Lib.Address>() { address };
       });
      var test = new AddressController(mockDataAccess.Object, mockAddressLogic.Object, mockLogger.Object);
      var newAddy = new Address.Api.Models.AddressModel
      {
        Street = "ooooooo",
        City = "oooooo",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      // act (send address to database)
      ActionResult<Address.Api.Models.AddressModel> address = await test.PostAddress(newAddy);

      // Assert correct address was received

      Assert.Equal(409, ((IStatusCodeActionResult)address.Result).StatusCode);
    }
  }
}
