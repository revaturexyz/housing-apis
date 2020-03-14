using System;
using Revature.Identity.Lib.Model;
using Xunit;

namespace Revature.Identity.Tests.LogicTests.Model
{
  public class TenantTesting
  {
    //declare an instance for testing
    private readonly TenantAccount _tenant = new TenantAccount();

    /// <summary>
    /// Test if the Tenant's name is null.
    /// </summary>
    [Fact]
    public void TenantNameNullException()
    {
      string nullString = null;

      Assert.ThrowsAny<ArgumentNullException>(() => _tenant.Name = nullString);
    }

    /// <summary>
    /// Test if a Tenant's name is an empty-string.
    /// </summary>
    [Fact]
    public void TenantNameEmptyException()
    {
      var emptyString = "";

      Assert.ThrowsAny<ArgumentException>(() => _tenant.Name = emptyString);
    }

    /// <summary>
    /// Test if the Tenant's email is null.
    /// </summary>
    [Fact]
    public void TenantEmailNullException()
    {
      string nullString = null;

      Assert.ThrowsAny<ArgumentNullException>(() => _tenant.Email = nullString);
    }

    /// <summary>
    /// Test if a Tenant's email is an empty-string.
    /// </summary>
    [Fact]
    public void TenantEmailEmptyException()
    {
      var emptyString = "";

      Assert.ThrowsAny<ArgumentException>(() => _tenant.Email = emptyString);
    }
  }
}
