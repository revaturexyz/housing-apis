using Revature.Lodging.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Services
{
  public interface IRoomRequest
  {
    public Task<IEnumerable<ApiRoom>> GetRooms(Guid complexId);
  }
}
