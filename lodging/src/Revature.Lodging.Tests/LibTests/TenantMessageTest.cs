using System;

using Xunit;
using BL = Revature.Lodging.Lib;

namespace Revature.Lodging.Tests.LibTests
{
  public class TenantMessageTest
  {
    private readonly Guid _newRoomId = Guid.Parse("349e5358-169a-4bc6-aa0f-c054952456dd");
    private readonly string _gender = "Male";

    /// <summary>
    /// A test to ensure that creating a tenant message which includes a tuple of (Guid, string)
    /// and an operation type is successful in creating.
    /// </summary>
    [Fact]
    public void TenantMessageShouldCreate()
    {
      // Tuple<Guid, string> tenantToInsert = new Tuple<Guid, string>(newRoomId, "Male");

      // TenantMessage tenantToInsert = new Tuple<Guid, string>(newRoomId, "Male");
      var newTenantMessage = new BL.Models.TenantMessage()
      {
        RoomId = _newRoomId,

        Gender = _gender,

        OperationType = BL.Models.OperationType.Create
      };

      Assert.True(newTenantMessage.Gender == _gender);
      Assert.True(newTenantMessage.OperationType == BL.Models.OperationType.Create);
    }
  }
}
