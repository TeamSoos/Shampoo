using DataLayer.Employee;
using ModelLayer.OrderMenu;
using ModelLayer.Tables;
using Npgsql;

namespace DataLayer.Tables;

public class TablesSQL : BaseSQL<Table> {
  public async Task<Table> get_one(int tableId) {
    var cmd = new NpgsqlCommand(
        "SELECT * FROM restaurant_table WHERE id = ($1);") {
        Parameters = { 
            new NpgsqlParameter { Value = tableId },
        }
    };
    return await QueryOne(cmd);
  }
  
  public async Task<List<Table>> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM restaurant_table");
    return await QueryMultiple(cmd);
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
  
  protected override Table ReadTables(NpgsqlDataReader reader) {
    
    return new Table {
      ID = (int)reader["id"],
      Number = (int)reader["number"],
      Status = Enum.Parse<TableStatus>(
          (string)reader["status"], ignoreCase: true),
      Employee = (int)reader["employee"]
    };
  }
}