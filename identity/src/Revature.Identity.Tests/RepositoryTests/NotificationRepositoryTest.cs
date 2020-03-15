using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Revature.Identity.DataAccess;
using Revature.Identity.DataAccess.Repositories;
using Xunit;

namespace Revature.Identity.Tests.Repository_Tests
{
  /// <summary>
  /// Tests for a Notification's data access layer and it's supporting database negotiation methods.
  /// </summary>
  public class NotificationRepositoryTest
  {
    /// <summary>
    /// Retrieve a notification from the database by way of a given Guid-Id.
    /// </summary>
    [Fact]
    public async void GetNotificationByIdAsyncTest()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();

      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("GetNotificationByProviderIdTest")
        .Options;
      var arrangeContext = new IdentityDbContext(options);
      arrangeContext.ProviderAccount.Add(mapper.MapProvider(helper.Providers[0]));
      arrangeContext.CoordinatorAccount.Add(mapper.MapCoordinator(helper.Coordinators[0]));
      arrangeContext.UpdateAction.Add(mapper.MapUpdateAction(helper.UpdateActions[0]));
      arrangeContext.Notification.Add(mapper.MapNotification(helper.Notifications[0]));
      arrangeContext.SaveChanges();
      var actContext = new IdentityDbContext(options);
      var repo = new GenericRepository(actContext, new Mapper());

      // Act
      var result = await repo.GetNotificationByIdAsync(helper.Notifications[0].NotificationId);

      // Assert
      Assert.Equal(helper.Notifications[0].NotificationId, result.NotificationId);
    }

    /// <summary>
    /// Test for adding a new notification to the database.
    /// </summary>
    [Fact]
    public void AddNewNotificationTest()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("AddNewNotificationTest")
        .Options;
      var actContext = new IdentityDbContext(options);
      var arrangeContext = new IdentityDbContext(options);
      var actRepo = new GenericRepository(actContext, new Mapper());
      var newNotification = helper.Notifications[0];

      // Act
      actRepo.AddNotification(newNotification);
      actContext.SaveChanges();

      // Assert
      var assertContext = new IdentityDbContext(options);
      var assertNotification = assertContext.Notification.First(p => p.CoordinatorId == newNotification.CoordinatorId);
      Assert.NotNull(assertNotification);
    }

    /// <summary>
    /// Update a given notification's entry in the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task UpdateNotificationAccountTestAsync()
    {
      // Arrange
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("UpdateNotificationAccountTestAsync")
        .Options;
      var arrangeContext = new IdentityDbContext(options);
      arrangeContext.ProviderAccount.Add(mapper.MapProvider(helper.Providers[0]));
      arrangeContext.CoordinatorAccount.Add(mapper.MapCoordinator(helper.Coordinators[0]));
      arrangeContext.UpdateAction.Add(mapper.MapUpdateAction(helper.UpdateActions[0]));
      arrangeContext.Notification.Add(mapper.MapNotification(helper.Notifications[0]));
      var repo = new GenericRepository(arrangeContext, new Mapper());

      // Act
      await repo.UpdateNotificationAsync(helper.Notifications[0]);
      arrangeContext.SaveChanges();

      // Assert
      var assertContext = new IdentityDbContext(options);
      var assertNotification = assertContext.Notification.First(p => p.CoordinatorId == helper.Notifications[0].CoordinatorId);
      Assert.Equal(helper.Notifications[0].CoordinatorId, assertNotification.CoordinatorId);
    }

    /// <summary>
    /// Delete a notification's entry from the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DeleteNotificationTestAsync()
    {
      // Assemble
      var helper = new TestHelper();
      var mapper = new Mapper();
      var options = new DbContextOptionsBuilder<IdentityDbContext>()
        .UseInMemoryDatabase("DeleteNotificationTestAsync")
        .Options;
      var assembleContext = new IdentityDbContext(options);
      var deleteNotification = mapper.MapNotification(helper.Notifications[0]);
      assembleContext.Add(deleteNotification);
      assembleContext.SaveChanges();
      var actContext = new IdentityDbContext(options);
      var repo = new GenericRepository(actContext, new Mapper());

      // Act
      await repo.DeleteNotificationByIdAsync(deleteNotification.NotificationId);

      // Assert
      var notification = actContext.Notification.ToList();
      Assert.DoesNotContain(deleteNotification, notification);
    }
  }
}
