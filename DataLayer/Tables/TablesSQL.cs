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
  
  
  public Table get_one_by_number(int tableNumber) {
    var cmd = new NpgsqlCommand(
        "SELECT * FROM restaurant_table WHERE number = ($1);") {
        Parameters = { 
            new NpgsqlParameter { Value = tableNumber },
        }
    };
    return QueryOneSync(cmd);
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
    string cmdString = ""; 
    // required due to enum
    switch (status) {
      case TableStatus.empty:
        cmdString = "UPDATE restaurant_table SET status = 'empty' WHERE id = @id;";
        break;
      case TableStatus.occupied:
        cmdString = "UPDATE restaurant_table SET status = 'occupied' WHERE id = @id;";
        break;
      case TableStatus.reserved:
        cmdString = "UPDATE restaurant_table SET status = 'reserved' WHERE id = @id;";
        break;
    }
    NpgsqlCommand cmd = new NpgsqlCommand(
        cmdString);
    cmd.Parameters.AddWithValue("id", table.ID);
    return cmd;
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
  public bool exists_number(object tableId) {
    var db = new Library.Database();
      
    // Query to check if the employee id exists in table
    // returns 1 if it does, 0 if not
    var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM restaurant_table WHERE number = ($1));"){
        Parameters = {
            new NpgsqlParameter { Value = tableId }
        }
    };
      
    // Set connection object for command
    cmd.Connection = db.Conn;
      
    // execute query manually
    bool valid = (bool)cmd.ExecuteScalar()!;
    db.Finalise();

    return valid;
  }
}