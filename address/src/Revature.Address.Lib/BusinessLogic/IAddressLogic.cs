using System.Threading.Tasks;

namespace Revature.Address.Lib.BusinessLogic
{
  public interface IAddressLogic
  {
    Task<bool> IsInRangeAsync(Address origin, Address destination, int distance);

    Task<bool> IsValidAddressAsync(Address address);

    Task<Address> NormalizeAddressAsync(Address address);
  }
}
