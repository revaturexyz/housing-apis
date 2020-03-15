using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Revature.Identity.Tests.ControllerTests
{
  /// <summary>
  /// Tests the API's Coordinator Controller.
  /// </summary>
  public class CoordinatorControllerTest
  {
    /// <summary>
    /// Test for coordinator retrieval based on their Guid-Id.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetCoordinatorByIdAsync()
    {
      var helper = new TestHelper();
      var coordinatorId = helper.Coordinators[0].CoordinatorId;

      helper.Repository
        .Setup(x => x.GetCoordinatorAccountByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.Run(() => helper.Coordinators.Where(c => c.CoordinatorId == coordinatorId).FirstOrDefault()));

      Assert.NotNull(await helper.CoordinatorAccountController.Get(coordinatorId) as OkObjectResult);
    }
  }
}
