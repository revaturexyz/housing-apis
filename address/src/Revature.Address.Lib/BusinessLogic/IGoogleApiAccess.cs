using System.Threading.Tasks;

namespace Revature.Address.Lib.BusinessLogic
{
  public interface IGoogleApiAccess
  {
    public Task<double> GetDistance(Address origin, Address destination, int distance);

    public Task<bool> IsValidAddressAsync(Address address);

    public Task<Address> NormalizeAddressAsync(Address address);
  }
}
