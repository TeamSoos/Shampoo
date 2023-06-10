namespace Logic.Models.Base; 

/// <summary>
/// BaseType - used as a base for types.
/// </summary>
public abstract class BaseType {

  /// <summary>
  /// GetId()
  /// Gets the ID field from object, if failed, raise exception. 
  /// </summary>
  /// <returns>ID of type int</returns>
  public int GetId {
    get {
      if (id != null) {
        return id;
      }

      throw new Exception("ID is not an initialized field. Failed to inherit or set id for type.");
    }
  }

  private int id;
  
  /// <summary>
  /// Get an Item from Database using it's ID
  /// </summary>
  /// <param name="id">id in db of the item</param>
  /// <returns>T[BaseType]</returns>
  public abstract T getByID<T>(int id);

  protected BaseType(int id) {
    this.id = id;
  }
}