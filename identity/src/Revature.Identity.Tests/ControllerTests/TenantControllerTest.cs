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
    /// Test for tenant retrieval based on their Guid-Id.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetTenantByIdAsync()
    {
      TestHelper helper = new TestHelper();
      Guid tenantId = helper.Tenants[0].TenantId;

      helper.Repository
        .Setup(x => x.GetTenantAccountByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.Run(() => helper.Tenants.Where(c => c.TenantId == tenantId).FirstOrDefault()));

      Assert.NotNull(await helper.TenantAccountController.Get(tenantId) as OkObjectResult);
    }
  }
}