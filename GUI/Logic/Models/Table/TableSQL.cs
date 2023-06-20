using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Table;

public class TableSQL {

  public async static Task<List<TableType>> get_all() {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT * FROM restaurant_table", db.Conn);


    var reader = await db.Query(cmd);
    List<TableType> tables = new List<TableType>();

    while (reader.Read())
    {
      TableType table = TableType.raw((int)reader["number"], (string)reader["status"]);
      tables.Add(table);
    }

    return tables;
  }
  
  public async static Task<DateTime> get_reservation_time(int id) {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT time FROM table_reservations WHERE table_id=($1)", db.Conn) {
        Parameters = {
            new() { Value = id }
        }
    };

    var reader = await db.Query(cmd);
    await reader.ReadAsync();
    DateTime table_data = reader.GetDateTime(0);
    await reader.CloseAsync();

    return table_data;
  }
  public async static Task<Dictionary<string, dynamic>> get_by_id(int id) {
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT * FROM restaurant_table WHERE id=($1)", db.Conn) {
        Parameters = {
            new() { Value = id }
        }
    };


    var reader = await db.Query(cmd);
    await reader.ReadAsync();
    // Consolidata data, this way we can shut off the connection in the same scope
    // This prevents memory leaks of passing around a reader object
    Dictionary<string, dynamic> table_data = new Dictionary<string, dynamic>() {
        { "number", reader["number"] },
        { "status", reader["status"] }
    };
    await reader.CloseAsync();

    return table_data;
  }
  public static void occupy_single(int tableId) {
    Library.Database db = new Library.Database();

    NpgsqlCommand cmd =
        new NpgsqlCommand(
            "UPDATE restaurant_table SET status = 'occupied' WHERE id = ($1);",
            db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = tableId },
            }
        };

    db.Store(cmd);
  }
  
  public static void free_single(int tableId) {
    Library.Database db = new Library.Database();

    NpgsqlCommand cmd =
        new NpgsqlCommand(
            "UPDATE restaurant_table SET status = 'empty' WHERE id = ($1);",
            db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = tableId },
            }
        };

    db.Store(cmd);
  }
}