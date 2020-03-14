using System.Text.Json;
using Humanizer;

namespace Revature.Address.Lib.BusinessLogic
{
  /// <summary>
  /// This class creates a new JSON naming policy based on snake case.
  /// </summary>
  public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
  {
    /// <summary>
    /// Configures the naming policy for Json Serializer.
    /// </summary>
    public override string ConvertName(string name)
    {
      return name.Underscore();
    }
  }
}
