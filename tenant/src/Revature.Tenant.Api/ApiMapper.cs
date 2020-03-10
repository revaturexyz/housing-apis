using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Revature.Tenant.Api.Models;
using Revature.Tenant.Lib.Models;

namespace Revature.Tenant.Api
{
  public static class ApiMapper
  {
    /// <summary>
    /// Maps a Model Batch to an Api Batch
    /// </summary>
    /// <param name="batch">A Model Batch</param>
    /// <returns>An Api Batch</returns>
    public static ApiBatch Map(Lib.Models.Batch batch)
    {
      return new ApiBatch
      {
        Id = batch.Id,
        BatchCurriculum = batch.BatchCurriculum,
        StartDate = batch.StartDate,
        EndDate = batch.EndDate,
        TrainingCenter = batch.TrainingCenter
      };
    }

    /// <summary>
    /// Maps an Api Batch to a Model Batch
    /// </summary>
    /// <param name="batch">An Api Batch</param>
    /// <returns>A Model Batch</returns>
    public static Lib.Models.Batch Map(ApiBatch batch)
    {
      var b = new Lib.Models.Batch
      {
        Id = batch.Id,
        BatchCurriculum = batch.BatchCurriculum,
        TrainingCenter = batch.TrainingCenter
      };
      b.SetStartAndEndDate(batch.StartDate, batch.EndDate);
      return b;
    }

    /// <summary>
    /// Maps a Model Car to an Api Car
    /// </summary>
    /// <param name="car">A Model Car</param>
    /// <returns>an Api Car</returns>
    public static ApiCar Map(Lib.Models.Car car)
    {
      return new ApiCar
      {
        Id = car.Id,
        LicensePlate = car.LicensePlate,
        Make = car.Make,
        Model = car.Model,
        Color = car.Color,
        Year = car.Year,
        State = car.State
      };
    }

    /// <summary>
    /// Maps an Api Car to a Model Car
    /// </summary>
    /// <param name="car">An Api Car</param>
    /// <returns>A Model Car</returns>
    public static Lib.Models.Car Map(ApiCar car)
    {
      return new Lib.Models.Car
      {
        Id = car.Id,
        LicensePlate = car.LicensePlate,
        Make = car.Make,
        Model = car.Model,
        Color = car.Color,
        Year = car.Year,
        State = car.State
      };
    }

    /// <summary>
    /// Maps a Models Tenant to an Api Tenant
    /// </summary>
    /// <param name="tenant">A Model Tenant</param>
    /// <returns>An Api Tenant</returns>
    public static ApiTenant Map(Lib.Models.Tenant tenant)
    {
      return new ApiTenant
      {
        Id = tenant.Id,
        Email = tenant.Email,
        Gender = tenant.Gender,
        FirstName = tenant.FirstName,
        LastName = tenant.LastName,
        AddressId = tenant.AddressId,
        RoomId = tenant.RoomId,
        CarId = tenant.CarId,
        BatchId = tenant.BatchId,
        TrainingCenter = tenant.TrainingCenter
      };
    }

    /// <summary>
    /// Maps an Api Tenant to a Model Tenant
    /// </summary>
    /// <param name="tenant">An Api Tenant</param>
    /// <returns>A Model Tenant</returns>
    public static Lib.Models.Tenant Map(ApiTenant tenant)
    {
      if(tenant.Id == null)
      {
        throw new ArgumentNullException("Tenant Id must be set");
      }
      return new Lib.Models.Tenant
      {
        Id = (Guid)tenant.Id,
        Email = tenant.Email,
        Gender = tenant.Gender,
        FirstName = tenant.FirstName,
        LastName = tenant.LastName,
        AddressId = (Guid)tenant.AddressId,
        RoomId = tenant.RoomId,
        CarId = tenant.CarId,
        BatchId = tenant.BatchId,
        TrainingCenter = tenant.TrainingCenter
      };
    }
  }
}
