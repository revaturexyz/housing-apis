using System;
using Revature.Identity.Lib.Model;
using Xunit;

namespace Revature.Identity.Tests.Logic_Tests.Model
{
  /// <summary>
  /// Tests for the business logic layer representation for the Provider.
  /// </summary>
  public class ProviderTesting
  {
    //instantiate a test object
    private readonly ProviderAccount _provider = new ProviderAccount();

    /// <summary>
    /// Test if the Provider's name is null.
    /// </summary>
    [Fact]
    public void ProviderNameNullException()
    {
      string nullString = null;

      Assert.ThrowsAny<ArgumentNullException>(() => _provider.Name = nullString);
    }

    /// <summary>
    /// Test if the Provider's name is a blank ("") or whitespaces ("  ") string.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void ProviderNameEmptyException(string value)
    {
      Assert.ThrowsAny<ArgumentException>(() => _provider.Name = value);
    }

    /// <summary>
    /// Test if the Provider's email is null.
    /// </summary>
    [Fact]
    public void ProviderEmailNullException()
    {
      string nullEmail = null;
      Assert.ThrowsAny<ArgumentNullException>(() => _provider.Email = nullEmail);
    }
    /// <summary>
    /// Test if the Provider's email is blank.
    /// </summary>
    [Fact]
    public void ProviderEmailEmptyException()
    {
      string emptyEmail = "";
      Assert.ThrowsAny<ArgumentException>(() => _provider.Email = emptyEmail);
    }

    /// <summary>
    /// Test if the Provider's email is not a proper email format.
    /// </summary>
    [Theory]
    [InlineData("  ")]
    [InlineData("aa")]
    [InlineData("b@a@c@domain.com")]
    public void ProviderEmailInvalidFormatException(string value)
    {
      Assert.ThrowsAny<FormatException>(() => _provider.Email = value);
    }
  }
}
