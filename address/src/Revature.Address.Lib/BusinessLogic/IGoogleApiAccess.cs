using System.Threading.Tasks;

namespace Revature.Address.Lib.BusinessLogic
{
  public interface IGoogleApiAccess
  {
    public Task<double> GetDistanceAsync(Address origin, Address destination);

    public Task<bool> IsValidAddressAsync(Address address);

    public Task<Address> NormalizeAddressAsync(Address address);
  }
}
