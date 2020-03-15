using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Revature.Identity.Tests.ControllerTests
{
  public class TenantControllerTest
  {
    /// <summary>
    /// Test for tenant retrieval based on their GUID.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetTenantByIdAsync()
    {
      var helper = new TestHelper();
      var tenantId = helper.Tenants[0].TenantId;

      helper.Repository
        .Setup(x => x.GetTenantAccountByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.Run(() => helper.Tenants.Where(c => c.TenantId == tenantId).FirstOrDefault()));

      Assert.NotNull(await helper.TenantAccountController.Get(tenantId) as OkObjectResult);
    }
  }
}
