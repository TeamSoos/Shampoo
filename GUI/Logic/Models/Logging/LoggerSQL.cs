using Logic.SQL;
using Npgsql;

namespace RoutedApp.Logic.Models.Logging; 

public class LoggerSQL {
  public static void addRecord(int employee, string action) {
    Library.Database db = new Library.Database();

    NpgsqlCommand cmd =
        new NpgsqlCommand(
            "INSERT INTO logging (employee, action) VALUES ($1, $2);",
            db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = employee },
                new NpgsqlParameter { Value = action },
            }
        };

    db.Store(cmd);
  }
}