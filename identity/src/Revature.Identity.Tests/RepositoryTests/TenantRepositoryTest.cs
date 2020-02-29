using Microsoft.EntityFrameworkCore;
using Revature.Account.DataAccess;
using Revature.Account.DataAccess.Repositories;
using Revature.Account.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Revature.Identity.Tests.RepositoryTests
{
  public class TenantRepositoryTest
  {
    /// <summary>
    /// Test for adding a new Provider entry to the database.
    /// </summary>
    [Fact]
    public void AddNewProviderAccountTest()
    {
      // Arrange
      var helper = new TestHelper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("AddNewProviderAccountTest")
        .Options;
      var actContext = new IdentityDbContext(options);
      var newProvider = helper.Providers[0];
      var actRepo = new GenericRepository(actContext, new Mapper());

      // Act
      actRepo.AddProviderAccountAsync(newProvider);
      actContext.SaveChanges();

      // Assert
      var assertContext = new IdentityDbContext(options);
      var assertProvider = assertContext.ProviderAccount.FirstOrDefault(p => p.ProviderId == newProvider.ProviderId);
      Assert.NotNull(assertProvider);
    }

    ///// <summary>
    ///// Test for updateing a given Provider's information within the database.
    ///// </summary>
    ///// <returns></returns>
    //[Fact]
    //public async Task UpdateProviderAccountTestAsync()
    //{
    //  // Arrange
    //  var helper = new TestHelper();
    //  var mapper = new Mapper();
    //  var options = new DbContextOptionsBuilder<IdentityDbContext>()
    //    .UseInMemoryDatabase("UpdateProviderAccountTestAsync")
    //    .Options;
    //  var arrangeContext = new IdentityDbContext(options);
    //  var arrangeProvider = helper.Providers[0];
    //  arrangeContext.ProviderAccount.Add(mapper.MapProvider(arrangeProvider));
    //  arrangeContext.SaveChanges();

    //  arrangeProvider.Name = "Robby";

    //  // Act
    //  var repo = new GenericRepository(arrangeContext, new Mapper());
    //  await repo.UpdateProviderAccountAsync(arrangeProvider);
    //  arrangeContext.SaveChanges();

    //  // Assert
    //  var assertContext = new IdentityDbContext(options);
    //  var assertProvider = assertContext.ProviderAccount.First(p => p.ProviderId == arrangeProvider.ProviderId);
    //  Assert.Equal(arrangeProvider.Name, assertProvider.Name);
    //}


    ///// <summary>
    ///// Retrieve a provider by way of a Guid Id from the database.
    ///// </summary>
    //[Fact]
    //public async void GetProviderByIdTest()
    //{
    //  // Arrange
    //  var helper = new TestHelper();
    //  var mapper = new Mapper();
    //  var options = new DbContextOptionsBuilder<IdentityDbContext>()
    //    .UseInMemoryDatabase("GetProviderByIdTest")
    //    .Options;
    //  var arrangeContext = new IdentityDbContext(options);

    //  arrangeContext.CoordinatorAccount.Add(mapper.MapCoordinator(helper.Coordinators[0]));
    //  arrangeContext.ProviderAccount.Add(mapper.MapProvider(helper.Providers[0]));
    //  arrangeContext.SaveChanges();
    //  var actContext = new IdentityDbContext(options);
    //  var repo = new GenericRepository(actContext, new Mapper());

    //  // Act
    //  var result = await repo.GetProviderAccountByIdAsync(helper.Providers[0].ProviderId);

    //  // Assert
    //  Assert.NotNull(result);
    //}


    ///// <summary>
    ///// Test the deletion of a given provider from the database.
    ///// </summary>
    ///// <returns></returns>
    //[Fact]
    //public async Task DeleteProviderTestAsync()
    //{
    //  //Assemble
    //  var helper = new TestHelper();
    //  var mapper = new Mapper();
    //  var options = new DbContextOptionsBuilder<IdentityDbContext>()
    //    .UseInMemoryDatabase("DeleteProviderTestAsync")
    //    .Options;
    //  var assembleContext = new IdentityDbContext(options);
    //  var deleteProvider = mapper.MapProvider(helper.Providers[2]);
    //  assembleContext.Add(deleteProvider);
    //  assembleContext.SaveChanges();
    //  var actContext = new IdentityDbContext(options);
    //  var repo = new GenericRepository(actContext, new Mapper());
    //  // Act
    //  await repo.DeleteProviderAccountAsync(deleteProvider.ProviderId);
    //  // Assert
    //  var provider = actContext.ProviderAccount.ToList();
    //  Assert.DoesNotContain(deleteProvider, provider);
    //}
  }
}
