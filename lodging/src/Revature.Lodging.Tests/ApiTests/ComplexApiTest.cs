using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Revature.Lodging.Api.Controllers;
using Revature.Lodging.Api.Models;
using Revature.Lodging.Api.Services;
using Revature.Lodging.Lib.Interface;
using Revature.Lodging.Lib.Models;
using Xunit;

namespace Revature.Lodging.Tests.ApiTests
{
  public class ComplexApiTest
  {
    /// <summary>
    /// This test is to test GetAllComplexAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void GetAllComplexAsyncTest()
    {
      var complex = new Complex
      {
        Id = Guid.NewGuid(),
        AddressId = Guid.NewGuid(),
        ProviderId = Guid.NewGuid(),
        ComplexName = "test",
        ContactNumber = "1234567892"
      };

      // setup
      var complexRepo = new Mock<IComplexRepository>();
      var amenityRepo = new Mock<IAmenityRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = new List<Complex>
      {
        complex
      };
      complexRepo.Setup(r => r.ReadComplexListAsync())
        .Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<ActionResult<IEnumerable<ApiComplex>>>(await controller.GetAllComplexesAsync());

      // assert
      Assert.IsAssignableFrom<ActionResult<IEnumerable<ApiComplex>>>(model);
    }

    /// <summary>
    /// This test is to test GetComplexByIdAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void GetComplexByIdAsyncTest()
    {
      // setup
      var complexId = Guid.NewGuid();
      var complexRepo = new Mock<IComplexRepository>();
      var amenityRepo = new Mock<IAmenityRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = new Complex();

      complexRepo.Setup(r => r.ReadComplexByIdAsync(complexId))
        .Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<ActionResult<ApiComplex>>(await controller.GetComplexByIdAsync(complexId));

      // assert
      Assert.IsAssignableFrom<ActionResult<ApiComplex>>(model);
    }

    /// <summary>
    /// This test is to test GetComplexByNameAndNumberAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void GetComplexByNameAndNumberAsyncTest()
    {
      // setup
      var name = "test1";
      var number = "1234567890";
      var complexRepo = new Mock<IComplexRepository>();
      var amenityRepo = new Mock<IAmenityRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = new Complex();

      complexRepo.Setup(r => r.ReadComplexByNameAndNumberAsync(name, number))
        .Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<ActionResult<ApiComplex>>(await controller.GetComplexByNameAndNumberAsync(name, number));

      // assert
      Assert.IsAssignableFrom<ActionResult<ApiComplex>>(model);
    }

    /// <summary>
    /// This test is to test GetComplexListByProviderIdAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void GetComplexListByProviderIdAsyncTest()
    {
      // setup
      var pId = Guid.NewGuid();
      var complex = new Complex
      {
        Id = Guid.NewGuid(),
        AddressId = Guid.NewGuid(),
        ProviderId = pId,
        ComplexName = "test",
        ContactNumber = "1234567892"
      };
      var complexRepo = new Mock<IComplexRepository>();
      var amenityRepo = new Mock<IAmenityRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = new List<Complex>
      {
        complex
      };
      complexRepo.Setup(r => r.ReadComplexByProviderIdAsync(pId))
        .Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<ActionResult<IEnumerable<ApiComplex>>>(await controller.GetComplexesByProviderId(pId));

      // assert
      Assert.IsAssignableFrom<ActionResult<IEnumerable<ApiComplex>>>(model);
    }

    /// <summary>
    /// This test is to test PutComplexAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void PutComplexAsyncTest()
    {
      var cId = Guid.NewGuid();
      var aId = Guid.NewGuid();
      var pId = Guid.NewGuid();
      var amId = Guid.NewGuid();
      var address = new ApiAddress
      {
        Id = aId,
        Street = "test ave",
        City = "dallas",
        State = "TX",
        Country = "USA",
        ZipCode = "76010"
      };
      var amenity = new Amenity
      {
        Id = amId,
        AmenityType = "name",
        Description = "description"
      };
      var amenities = new List<Amenity>
      {
        amenity
      };
      var apiComplex = new ApiComplex
      {
        ComplexId = cId,
        Address = address,
        ProviderId = pId,
        ComplexName = "Liv+",
        ContactNumber = "1234567890",
        ComplexAmenities = amenities
      };
      var complex = new Complex
      {
        Id = cId,
        AddressId = aId,
        ProviderId = pId,
        ComplexName = "Liv+",
        ContactNumber = "1234567890"
      };
      var ac = new ComplexAmenity
      {
        Id = Guid.NewGuid(),
        AmenityId = amId,
        ComplexId = cId
      };
      var amenityRepo = new Mock<IAmenityRepository>();
      var complexRepo = new Mock<IComplexRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = true;

      /*complex*/
      amenityRepo.Setup(r => r.DeleteAmenityComplexAsync(cId))
.Returns(Task.FromResult(res));
      complexRepo.Setup(r => r.UpdateComplexAsync(complex))
        .Returns(Task.FromResult(res));
      /*complex*/
      amenityRepo.Setup(c => c.ReadAmenityListAsync())
.Returns(Task.FromResult(amenities));
      /*complex*/
      amenityRepo.Setup(p => p.CreateAmenityComplexAsync(ac))
.Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<StatusCodeResult>(await controller.UpdateComplexAsync(apiComplex));

      // assert
      Assert.IsAssignableFrom<StatusCodeResult>(model);
    }

    /// <summary>
    /// This test is to test DeleteComplexAsync in Complex Api.
    /// </summary>
    [Fact]
    public async void DeleteComplexAsyncTest()
    {
      var cId = Guid.NewGuid();
      var aId = Guid.NewGuid();
      var amenityRepo = new Mock<IAmenityRepository>();
      var complexRepo = new Mock<IComplexRepository>();
      var logger = new Mock<ILogger<ComplexController>>();
      var ar = new Mock<IAddressRequest>();
      var res = true;

      /*complex*/
      amenityRepo.Setup(r => r.DeleteAmenityComplexAsync(cId))
.Returns(Task.FromResult(res));
      complexRepo.Setup(r => r.DeleteComplexAsync(cId))
        .Returns(Task.FromResult(res));

      // act
      var controller = new ComplexController(complexRepo.Object, logger.Object, ar.Object, amenityRepo.Object);
      var model = Assert.IsAssignableFrom<StatusCodeResult>(await controller.DeleteComplexByIdAsync(cId));

      // assert
      Assert.IsAssignableFrom<StatusCodeResult>(model);
    }
  }
}
