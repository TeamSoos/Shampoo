using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GUI.Logic.Models.Table;
using Logic.Models.Base;
using RoutedApp.Logic.Models.Employee;

namespace GUI.Logic.Models.Employee;

public enum Jobs {
  admin,
  waiter,
  chef
}

public class EmployeeType : BaseType {

  public Jobs job;
  public int id;
  public string name;
  
  public EmployeeType(int id) : base(id) {
    Task.Run(async () =>
    {
      Dictionary<string, dynamic> table_data = await EmployeeSQL.get_by_id(id);
      name = table_data["name"];
      id = table_data["id"];
      // error handling goes to shit but oh well
      job = Enum.Parse<Jobs>(table_data["job"], ignoreCase: true);
    }).Wait();
  }

  public static async Task<List<EmployeeType>> getAll() {
    /*
     * This is poo poo
     * It handles multiple async calls in two calls
     * One to get all of the tables
     * Second to create an instance of the TableType.
     *
     * This can be optimized, but I'm lazy and short on time
     */
    return await EmployeeSQL.get_all();
  }
  
  public static async Task<List<EmployeeType>> getAll(string job) {
    /*
     * This is poo poo
     * It handles multiple async calls in two calls
     * One to get all of the tables
     * Second to create an instance of the TableType.
     *
     * This can be optimized, but I'm lazy and short on time
     */
    return await EmployeeSQL.get_all_by_job(job);
  }

  public override T getByID<T>(int id) {
    throw new NotImplementedException();
  }
}