using DataLayer.Employee;
using ModelLayer.OrderMenu;
using ModelLayer.Tables;
using Npgsql;

namespace DataLayer.Tables;

public class TablesSQL : BaseSQL<Table> {
  public Table get_one(int tableId) {
    var cmd = new NpgsqlCommand(
        "SELECT * FROM restaurant_table WHERE id = ($1);") {
        Parameters = { 
            new NpgsqlParameter { Value = tableId },
        }
    };
    return QueryOneSync(cmd);
  }
  public async Task<Table> get_one_async(int tableId) {
    var cmd = new NpgsqlCommand(
        "SELECT * FROM restaurant_table WHERE id = ($1);") {
        Parameters = { 
            new NpgsqlParameter { Value = tableId },
        }
    };
    return await QueryOne(cmd);
  }
  
  public List<Table> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM restaurant_table ORDER BY id ASC");
    return QueryMultiple(cmd);
  }
  public async Task<List<Table>> get_all_async() {
    var cmd = new NpgsqlCommand("SELECT * FROM restaurant_table ORDER BY id ASC");
    return await QueryMultipleAsync(cmd);
  }
  
  public void Reserve(Table table) {
    this.Store(
        ChangeStatusCMD(table, TableStatus.reserved)    
    );
  }

  public void Occupy(Table table) {
    this.Store(
        ChangeStatusCMD(table, TableStatus.occupied)    
    );
  }
  
  public void Free(Table table) {
    this.Store(
        ChangeStatusCMD(table, TableStatus.empty)    
    );
  }

  private NpgsqlCommand ChangeStatusCMD(Table table, TableStatus status) {
    return new NpgsqlCommand(
        "UPDATE restaurant_table SET status = ($1) WHERE id = ($2);") {
        Parameters = { 
            new NpgsqlParameter { Value = status.ToString() },
            new NpgsqlParameter { Value = table.ID },
        }
    };
  }
  
  public bool exists(int id) {
    var db = new Library.Database();
      
    // Query to check if the employee id exists in table
    // returns 1 if it does, 0 if not
    var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM restaurant_table WHERE id = ($1));"){
        Parameters = {
            new NpgsqlParameter { Value = id }
        }
    };
      
    // Set connection object for command
    cmd.Connection = db.Conn;
      
    // execute query manually
    bool valid = (bool)cmd.ExecuteScalar()!;
    db.Finalise();

    return valid;
  }
  
  protected override Table ReadTables(NpgsqlDataReader reader) {
    EmployeeSQL employeeDB = new EmployeeSQL();
    
    return new Table {
      ID = (int)reader["id"],
      Number = (int)reader["number"],
      Status = Enum.Parse<TableStatus>(
          (string)reader["status"], ignoreCase: true),
      Employee = employeeDB.get_one((int)reader["waiter"])
    };
  }
}