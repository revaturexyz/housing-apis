using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Revature.Address.Lib.BusinessLogic;
using Xunit;

namespace Revature.Address.Tests.Lib.Tests
{
  public class GoogleApiAccessTests
  {
    [Fact]
    public void CheckAddressFormatting()
    {
      var mockHttpClient = new Mock<HttpClient>();
      var mockHttpClientFactory = new Mock<IHttpClientFactory>();
      mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
        .Returns(mockHttpClient.Object);
      var mockConfiguration = new Mock<IConfiguration>();
      var logger = new NullLogger<GoogleApiAccess>();

      var sut = new GoogleApiAccess(mockHttpClientFactory.Object, mockConfiguration.Object, logger);

      var newAddy = new Address.Lib.Address
      {
        Id = new Guid("566e1a61-c283-4d33-9b9b-9a981393cf2b"),
        Street = "1100 N E St",
        City = "Arlington",
        State = "Texas",
        Country = "US",
        ZipCode = "76010"
      };

      var result = sut.UrlEncodeAddress(newAddy);
      Assert.Equal("1100%20N%20E%20St%20Arlington%2C%20Texas%20US%2076010", result);
    }
  }
}
