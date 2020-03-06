using Microsoft.EntityFrameworkCore;
using Revature.Identity.DataAccess;
using Revature.Identity.DataAccess.Repositories;
using Xunit;

namespace Revature.Identity.Tests.Repository_Tests
{
  /// <summary>
  /// Tests for the Coordinator's data access layer and it's supporting database negotiation methods.
  /// </summary>
  public class CoordinatorRepositoryTest
  {
    [Fact]
    public async void GetCoordinatorByIdTest()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("GetCoordinatorByIdTest")
        .Options;
      var arrangeContext = new IdentityDbContext(options);
      var testCoordinator = helper.Coordinators[0];
      var testId = testCoordinator.CoordinatorId;
      arrangeContext.CoordinatorAccount.Add(mapper.MapCoordinator(testCoordinator));
      arrangeContext.SaveChanges();
      var actContext = new IdentityDbContext(options);
      var repo = new GenericRepository(actContext, new Mapper());

      // Act
      var result = await repo.GetCoordinatorAccountByIdAsync(testId);

      // Assert
      Assert.Equal(testId, result.CoordinatorId);
    }
  }
}
