namespace Revature.Address.Lib.Interfaces
{
  /// <summary>
  /// Interface for Mapper class to facilitate dependency injection,
  /// Maps DataAccess address objects to Business Library address objects
  /// and vice versa
  /// </summary>
  public interface IMapper
  {

    public Lib.Address MapAddress(Revature.Address.DataAccess.Entities.Address address);
    public Revature.Address.DataAccess.Entities.Address MapAddress(Lib.Address address);
  }
}
