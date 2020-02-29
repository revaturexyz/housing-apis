using Microsoft.AspNetCore.Mvc;
using Moq;
using Revature.Account.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Revature.Identity.Tests.ControllerTests
{
  public class TenantControllerTest
  {
    /// <summary>
    /// Test for coordinator retrieval based on their Guid-Id.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetTenantByIdAsync()
    {
      TestHelper helper = new TestHelper();
      Guid coordinatorId = helper.Coordinators[0].CoordinatorId;

      helper.Repository
        .Setup(x => x.GetCoordinatorAccountByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.Run(() => helper.Coordinators.Where(c => c.CoordinatorId == coordinatorId).FirstOrDefault()));

      Assert.NotNull(await helper.CoordinatorAccountController.Get(coordinatorId) as OkObjectResult);
    }
  }
}
