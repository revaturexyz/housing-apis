using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Revature.Lodging.Api.Controllers;
using Revature.Lodging.Lib.Interface;
using Xunit;

namespace Revature.Lodging.Tests.ApiTests
{
  public class TenantControllerTests
  {
    /// <summary>
    /// Unit test for GetAsync method in TenantController.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetAsyncShouldReturnRoomList()
    {
      // arrange
      var mockRepo = new Mock<IRoomRepository>();
      var mockLogger = new Mock<ILogger<TenantController>>();
      var controller = new TenantController(mockRepo.Object, mockLogger.Object);

      var gender = "male";
      var dateTime = default(DateTime);

      // act
      var result = await controller.GetAsync(gender, dateTime);

      // assert
      Assert.NotNull(result);
      Assert.IsAssignableFrom<OkObjectResult>(result);
    }
  }
}
