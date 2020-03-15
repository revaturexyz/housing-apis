using System;
using Revature.Tenant.Lib.Models;
using Xunit;

namespace Revature.Tenant.Tests.LibTests
{
  public class RoomMessageTest
  {
    private Guid _roomId = Guid.NewGuid();

    [Fact]
    public void RoomMessageShouldCreate()
    {
      string gender = "Female";

      var result = new RoomMessage()
      {
        Gender = gender,
        RoomId = _roomId,
        OperationType = 0
      };

      Assert.NotNull(result);
      Assert.True(result.Gender == gender);
      Assert.True(result.RoomId == _roomId);
      Assert.True(result.OperationType == 0);
    }

    [Fact]
    public void RoomMessageShouldCheckValidation()
    {
      Assert.Throws<ArgumentException>(() => new RoomMessage() { RoomId = Guid.Empty });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { Gender = null });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { Gender = string.Empty });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { Gender = "\n" });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { Gender = "        " });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { OperationType = -1 });
      Assert.Throws<ArgumentException>(() => new RoomMessage() { OperationType = 2 });
    }
  }
}
