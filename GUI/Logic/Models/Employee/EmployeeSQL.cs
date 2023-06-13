using System.Collections.Generic;
using System.Threading.Tasks;
using GUI.Logic.Models.Employee;
using GUI.Logic.Models.Table;
using Logic.SQL;
using Npgsql;

namespace RoutedApp.Logic.Models.Employee; 

public class EmployeeSQL {

  public async static Task<List<EmployeeType>> get_all() {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT id FROM employees", db.Conn);
    
    var reader = await db.Query(cmd);
    List<EmployeeType> employees = new List<EmployeeType>();

    while (reader.Read())
    {
      EmployeeType employee = new EmployeeType((int)reader["id"]);
      employees.Add(employee);
    }

    return employees;
  }
  
  public async static Task<List<EmployeeType>> get_all_by_job(string job) {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT id FROM employees where job=($1)", db.Conn){
        Parameters = {
            new() { Value = job }
        }
    };
    
    var reader = await db.Query(cmd);
    List<EmployeeType> employees = new List<EmployeeType>();

    while (reader.Read())
    {
      EmployeeType employee = new EmployeeType((int)reader["id"]);
      employees.Add(employee);
    }

    return employees;
  }
  public async static Task<Dictionary<string, dynamic>> get_by_id(int id) {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT * FROM employees WHERE id=($1)", db.Conn) {
        Parameters = {
            new() { Value = id }
        }
    };


    var reader = await db.Query(cmd);
    await reader.ReadAsync();
    // Consolidata data, this way we can shut off the connection in the same scope
    // This prevents memory leaks of passing around a reader object
    Dictionary<string, dynamic> employee_data = new Dictionary<string, dynamic>() {
        { "id", reader["id"] },
        { "name", reader["name"] },
        { "job", reader["job"] }
    };
    await reader.CloseAsync();

    return employee_data;
  }
}