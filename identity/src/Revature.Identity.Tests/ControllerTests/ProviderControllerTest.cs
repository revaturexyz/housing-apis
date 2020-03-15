using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Revature.Identity.Lib.Model;
using Xunit;

namespace Revature.Identity.Tests.ControllerTests
{
  /// <summary>
  /// Tests for the Provider Controller.
  /// </summary>
  public class ProviderControllerTest
  {
    /// <summary>
    /// Test for Provider retrieval based on their Guid-Id.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetProviderByIdAsync()
    {
      var helper = new TestHelper();
      var providerId = helper.Providers[0].ProviderId;

      helper.Repository
        .Setup(x => x.GetProviderAccountByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.FromResult(helper.Providers[0]));

      Assert.NotNull(await helper.ProviderAccountController.Get(providerId) as OkObjectResult);
    }

    /// <summary>
    /// Test for a sucessful provider-account-update.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task UpdateProviderAccountAsyncSuccessfulVerificationAsync()
    {
      var helper = new TestHelper();
      var providerId = helper.Notifications[0].ProviderId;
      var updatedProvider = helper.Providers[0];
      updatedProvider.Name = "New Name";

      helper.Repository
        .Setup(x => x.GetProviderAccountByIdAsync(It.IsAny<Guid>()))
          .Returns(Task.FromResult(helper.Providers[0]))
          .Verifiable();
      helper.Repository
        .Setup(x => x.UpdateProviderAccountAsync(It.IsAny<ProviderAccount>()))
          .Returns(Task.FromResult(true))
          .Verifiable();

      var updatedResult = await helper.ProviderAccountController.Put(providerId, updatedProvider);

      helper.Repository
        .Verify();
    }

    /// <summary>
    /// Test for a successful provider account deletion.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DeleteProviderAccountAsyncSuccessfulVerificationAsync()
    {
      var helper = new TestHelper();
      var providerId = helper.Providers[0].ProviderId;

      helper.Repository
        .Setup(x => x.GetProviderAccountByIdAsync(It.IsAny<Guid>()))
          .Returns(Task.FromResult(helper.Providers[0]))
          .Verifiable();
      helper.Repository
        .Setup(x => x.DeleteProviderAccountAsync(It.IsAny<Guid>()))
          .Returns(Task.FromResult(true))
          .Verifiable();

      var deleted = await helper.ProviderAccountController.Delete(providerId);

      helper.Repository
        .Verify();
    }
  }
}
