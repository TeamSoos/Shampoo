using Logic.Models.Base;

namespace GUI.Logic.Models.Table;


public enum Status {
  empty,
  occupied,
  reserved
}

public class TableType : BaseType {
  
  public int number;
  public Status status;
  

  public TableType(int id) : base(id) {
    Task.Run(async () =>
    {
      Dictionary<string, dynamic> table_data = await TableSQL.get_by_id(id);
      number = table_data["number"];
      // error handling goes to shit but oh well
      status = Enum.Parse<Status>(table_data["status"], ignoreCase: true);
    }).Wait();
  }

  public static async Task<List<TableType>> getAll() {
    /*
     * This is poo poo
     * It handles multiple async calls in two calls
     * One to get all of the tables
     * Second to create an instance of the TableType.
     *
     * This can be optimized, but I'm lazy and short on time
     */
    return await TableSQL.get_all();
  }

  public override T getByID<T>(int id) {
    throw new NotImplementedException();
  }
}