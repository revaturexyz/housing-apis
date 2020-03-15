using System;
using System.Threading.Tasks;
using Revature.Lodging.Api.Models;

namespace Revature.Lodging.Api.Services
{
  public interface IAddressRequest
  {
    public Task<ApiAddress> PostAddressAsync(ApiAddress item);

    public Task<ApiAddress> GetAddressAsync(Guid addressId);
  }
}
