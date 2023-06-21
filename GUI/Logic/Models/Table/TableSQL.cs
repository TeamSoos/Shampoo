using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GUI.Logic.Models.Order;
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

    db.Finalize();

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
    db.Finalize();

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
    db.Finalize();

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
  public static async Task<List<string>> get_orders_by_id(int id) {
      Library.Database db = new Library.Database();

      var cmd = new NpgsqlCommand("SELECT status, paid FROM orders WHERE table_id=($1)", db.Conn) {
              Parameters = {
                      new() { Value = id }
              }
      };

      List<string> orders = new List<string>();

      var reader = await db.Query(cmd);
      
      while (await reader.ReadAsync()) {
          if ((bool)reader["paid"]) 
              continue;
          
          orders.Add(reader["status"].ToString()!);
          Console.WriteLine($"{id} -> {reader["status"].ToString()!}");
      }
      
      await reader.CloseAsync();
      
      db.Finalize();

      return orders;
  }
  public static async Task<List<OrderType>> get_ordertypes_by_id(int tableId) {
      Library.Database db = new Library.Database();

      var cmd = new NpgsqlCommand("SELECT id, status, paid FROM orders WHERE table_id=($1)", db.Conn) {
              Parameters = {
                      new() { Value = tableId }
              }
      };

      List<OrderType> orders = new List<OrderType>();

      var reader = await db.Query(cmd);
      
      while (await reader.ReadAsync()) {
          if ((bool)reader["paid"]) 
              continue;

          orders.Add(new OrderType((int)reader["id"], (bool)reader["paid"], reader["status"].ToString() == "none"));
      }
      
      await reader.CloseAsync();
      
      db.Finalize();

      return orders;
  }
}