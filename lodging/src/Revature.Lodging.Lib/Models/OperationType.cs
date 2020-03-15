namespace Revature.Lodging.Lib.Models
{
  /// <summary>
  /// The operation type is what CRUD operation you are sending
  /// We are only receiving Create, Update, and Delete.
  /// For the OperationType it goes as follows
  /// Create: 0, Update: 1, Delete: 2.
  /// </summary>
  public enum OperationType
  {
    Create,
    Delete,
    Update,
    DeleteCom
  }
}
