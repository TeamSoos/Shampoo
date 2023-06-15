using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Table;

public class TableSQL {

    public static async Task<List<TableType>> get_all() {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT id FROM restaurant_table", db.Conn);


        NpgsqlDataReader reader = await db.Query(cmd);
        List<TableType> tables = new List<TableType>();

        while (reader.Read()) {
            TableType table = new TableType((int)reader["id"]);
            tables.Add(table);
        }

        return tables;
    }

    public static async Task<Dictionary<string, dynamic>> get_by_id(int id) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM restaurant_table WHERE id=($1)", db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = id }
            }
        };


        NpgsqlDataReader reader = await db.Query(cmd);
        await reader.ReadAsync();
        // Consolidata data, this way we can shut off the connection in the same scope
        // This prevents memory leaks of passing around a reader object
        Dictionary<string, dynamic> table_data = new Dictionary<string, dynamic> {
            { "number", reader["number"] },
            { "status", reader["status"] }
        };
        await reader.CloseAsync();

        return table_data;
    }
}