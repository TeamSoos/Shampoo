using ModelLayer.Tables;
using DataLayer.Tables;
using Npgsql;

namespace DataLayer.Order; 

public class OrderSQL : BaseSQL<ModelLayer.Order> {
  public ModelLayer.Order get_one(int id) {
    var cmd = new NpgsqlCommand("SELECT * FROM orders WHERE id = ($1)"){
        Parameters = {
            new NpgsqlParameter { Value = id }
        }
    }; 
    
    return QueryOneSync(cmd);
  }
  
  public List<ModelLayer.Order> get_all() {
    var cmd = new NpgsqlCommand("SELECT * FROM orders");
    return QueryMultiple(cmd);
  }
  
  public List<ModelLayer.Order> get_all_by_table(Table table) {
    var cmd = new NpgsqlCommand("SELECT * FROM orders WHERE table_id = ($1)"){
        Parameters = {
            new NpgsqlParameter { Value = table.ID }
        }
    }; 

    return QueryMultiple(cmd);
  }

  protected override ModelLayer.Order ReadTables(NpgsqlDataReader reader) {
    TablesSQL tablesSql = new TablesSQL();
    return new ModelLayer.Order {
        ID = (int)reader["id"],
        Table = tablesSql.get_one((int)reader["table_id"]),
        Paid = (bool)reader["paid"],
        Status = (string)reader["status"]
    };
  }
}