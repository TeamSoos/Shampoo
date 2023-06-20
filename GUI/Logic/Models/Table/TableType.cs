using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Models.Base;

namespace GUI.Logic.Models.Table;


public enum Status {
    empty,
    occupied,
    reserved
}

public class TableType  {
  
  public int number;
  public Status status;
  private TableType(int number, string status){
    
    this.number = number;
    // error handling goes to shit but oh well
    this.status = Enum.Parse<Status>(status, ignoreCase: true);
  }


  public TableType(int id)  {
    Task.Run(async () =>
    {
      Dictionary<string, dynamic> table_data = await TableSQL.get_by_id(id);
      number = table_data["number"];
      // error handling goes to shit but oh well
      status = Enum.Parse<Status>(table_data["status"], ignoreCase: true);
    }).Wait();
  }

  public static TableType raw(int number, string status) {
    return new TableType(number, status);
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
  
  public static void occupy_single(int table_id) {
    TableSQL.occupy_single(table_id);
  }
  public static void free_single(int table_id) {
    TableSQL.free_single(table_id);
  }

  public static async Task<List<string>> getOrders(int table_id) {
    return await TableSQL.get_orders_by_id(table_id);
  }

}