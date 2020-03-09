using Revature.Lodging.Api.Models;
using System;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Services
{
  public interface IAddressRequest
  {
    public Task<ApiAddress> PostAddressAsync(ApiAddress item);

    public Task<ApiAddress> GetAddressAsync(Guid addressId);
  }
}
