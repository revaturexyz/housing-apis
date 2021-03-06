using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Revature.Identity.DataAccess;
using Revature.Identity.DataAccess.Repositories;
using Xunit;

namespace Revature.Identity.Tests.RepositoryTests
{
  public class TenantRepositoryTest
  {
    /// <summary>
    /// Test for adding a new Tenant entry to the database.
    /// </summary>
    [Fact]
    public void AddNewTenantAccountTest()
    {
      // Arrange
      var helper = new TestHelper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("AddNewTenantAccountTest")
        .Options;
      var actContext = new IdentityDbContext(options);
      var newTenant = helper.Tenants[0];
      var actRepo = new GenericRepository(actContext, new Mapper());

      // Act
      actRepo.AddTenantAccount(newTenant);
      actContext.SaveChanges();

      // Assert
      var assertContext = new IdentityDbContext(options);
      var assertTenant = assertContext.TenantAccount.FirstOrDefault(p => p.TenantId == newTenant.TenantId);
      Assert.NotNull(assertTenant);
    }

    /// <summary>
    /// Test for updating a given Tenant's information within the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task UpdateTenantAccountTestAsync()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("UpdateTenantAccountTestAsync")
        .Options;
      var arrangeContext = new IdentityDbContext(options);
      var arrangeTenant = helper.Tenants[0];
      arrangeContext.TenantAccount.Add(mapper.MapTenant(arrangeTenant));
      arrangeContext.SaveChanges();

      // Act
      var repo = new GenericRepository(arrangeContext, new Mapper());
      await repo.UpdateTenantAccountAsync(arrangeTenant);
      arrangeContext.SaveChanges();

      // Assert
      var assertContext = new IdentityDbContext(options);
      var assertTenant = assertContext.TenantAccount.First(p => p.TenantId == arrangeTenant.TenantId);
      Assert.Equal(arrangeTenant.Name, assertTenant.Name);
    }

    /// <summary>
    /// Retrieve a Tenant by way of a Guid Id from the database.
    /// </summary>
    [Fact]
    public async void GetTenantByIdTest()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("GetTenantByIdTest")
        .Options;
      var arrangeContext = new IdentityDbContext(options);

      arrangeContext.TenantAccount.Add(mapper.MapTenant(helper.Tenants[0]));
      arrangeContext.SaveChanges();
      var actContext = new IdentityDbContext(options);
      var repo = new GenericRepository(actContext, new Mapper());

      // Act
      var result = await repo.GetTenantAccountByIdAsync(helper.Tenants[0].TenantId);

      // Assert
      Assert.NotNull(result);
    }

    /// <summary>
    /// Test the deletion of a given tenant from the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DeleteTenantTestAsync()
    {
      // Assemble
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("DeleteTenantTestAsync")
        .Options;
      var assembleContext = new IdentityDbContext(options);
      var deleteTenant = mapper.MapTenant(helper.Tenants[2]);
      assembleContext.Add(deleteTenant);
      assembleContext.SaveChanges();
      var actContext = new IdentityDbContext(options);
      var repo = new GenericRepository(actContext, new Mapper());

      // Act
      await repo.DeleteTenantAccountAsync(deleteTenant.TenantId);

      // Assert
      var tenant = actContext.TenantAccount.ToList();
      Assert.DoesNotContain(deleteTenant, tenant);
    }
  }
}
