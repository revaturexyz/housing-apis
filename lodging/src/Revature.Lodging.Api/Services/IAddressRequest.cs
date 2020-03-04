using Revature.Lodging.Api.Models;
using System;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Services
{
  public interface IAddressRequest
  {
    public Task<ApiComplexAddress> PostAddressAsync(ApiComplexAddress item);

    public Task<ApiComplexAddress> GetAddressAsync(Guid addressId);
  }
}
