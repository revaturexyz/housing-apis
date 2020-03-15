using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Revature.Tenant.DataAccess;
using Revature.Tenant.DataAccess.Repository;
using Xunit;

namespace Revature.Tenant.Tests.DataTests
{
  /// <summary>
  /// Unit tests for data access methods in TenantRoomRepository class.
  /// </summary>
  public class TenantRoomRepositoryTest
  {
    /// <summary>
    /// GetTenantsByRoomIdAsync should return a list of tenants.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetTenantsByRoomIdShouldReturnList()
    {
      var options = TestDbInitializer.InitializeDbOptions("GetTenantsByRoomIdShouldReturnList");
      using var context = TestDbInitializer.CreateTestDb(options);
      var mapper = new Mapper();
      var repo = new TenantRoomRepository(context, mapper);

      var tenant = new DataAccess.Entities.Tenant()
      {
        Id = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9ac7d"),
        Email = "firstname@email.com",
        Gender = "Male",
        FirstName = "Clary",
        LastName = "Colton",
        AddressId = Guid.Parse("fa4d6c6e-9650-45c9-8c6b-5aebd3f9a67c"),
        RoomId = Guid.Parse("fa4d6c6e-9650-44c9-5c6b-5aebd3f9a67c"),
        CarId = 3,
        BatchId = 3,
        TrainingCenter = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9a67d"),
        Car = new DataAccess.Entities.Car
        {
          Id = 3,
          LicensePlate = "LicensePlate",
          Make = "Make",
          Model = "Model",
          Color = "Color",
          Year = "Year",
          State = "TX"
        },
        Batch = new DataAccess.Entities.Batch
        {
          Id = 3,
          BatchCurriculum = "C#",
          TrainingCenter = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9a67d"),
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddDays(3)
        }
      };

      await context.Tenant.AddAsync(tenant);
      await context.SaveChangesAsync();

      var result = await repo.GetTenantsByRoomIdAsync(Guid.Parse("fa4d6c6e-9650-44c9-5c6b-5aebd3f9a67c"));

      Assert.NotNull(result);
      Assert.Equal("Clary", result.First().FirstName);

      result = await repo.GetTenantsByRoomIdAsync(Guid.NewGuid());
      Assert.True(result.Count == 0);
    }

    /// <summary>
    /// GetRoomLessTenants should return a list of Tenants who have not yet been assigned a room.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetRoomlessTenantsShouldReturnList()
    {
      var options = TestDbInitializer.InitializeDbOptions("GetRoomlessTenantsShouldReturnList");
      using var context = TestDbInitializer.CreateTestDb(options);
      var mapper = new Mapper();
      var repo = new TenantRoomRepository(context, mapper);

      var tenant = new DataAccess.Entities.Tenant()
      {
        Id = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9ac7d"),
        Email = "firstname@email.com",
        Gender = "Female",
        FirstName = "Lana",
        LastName = "Del Ray",
        AddressId = Guid.Parse("fa4d6c6e-9650-45c9-8c6b-5aebd3f9a67c"),
        RoomId = null,
        CarId = 3,
        BatchId = 3,
        TrainingCenter = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9a67d"),
        Car = new DataAccess.Entities.Car
        {
          Id = 3,
          LicensePlate = "LicensePlate",
          Make = "Make",
          Model = "Model",
          Color = "Color",
          Year = "Year",
          State = "TX"
        },
        Batch = new DataAccess.Entities.Batch
        {
          Id = 3,
          BatchCurriculum = "C#",
          TrainingCenter = Guid.Parse("fa4d6c6e-9650-44c9-8c6b-5aebd3f9a67d"),
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddDays(3)
        }
      };

      await context.Tenant.AddAsync(tenant);
      await context.SaveChangesAsync();

      var result = await repo.GetRoomlessTenantsAsync();

      Assert.NotNull(result);
      Assert.NotNull(result.First(r => r.FirstName == "Lana"));
      Assert.IsType<List<Lib.Models.Tenant>>(result);
    }
  }
}
