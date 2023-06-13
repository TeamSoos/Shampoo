using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using GUI.Logic.Models.Table;
using Logic.SQL;
using Npgsql;

namespace Logic.Models.Item; 

public class ItemSQL {
    public async static Task<List<ItemType>> GetAll()
    {
        Library.Database db = new Library.Database();

        var cmd = new NpgsqlCommand("SELECT id, name, count FROM items ORDER BY id ASC", db.Conn);


        var reader = await db.Query(cmd);
        List<ItemType> items = new List<ItemType>();

        while (reader.Read())
        {
            ItemType item = ItemType.raw((int)reader["id"], (string)reader["name"], (int)reader["count"]);
            items.Add(item);
        }

        return items;
    }

    public static async Task<Dictionary<string, dynamic>> get_by_id(int id)
    {Library.Database db = new Library.Database();

        var cmd = new NpgsqlCommand("SELECT * FROM items WHERE id=($1)", db.Conn) {
            Parameters = {
                new() { Value = id }
            }
        };


        var reader = await db.Query(cmd);
        await reader.ReadAsync();
        
        Dictionary<string, dynamic> table_data = new Dictionary<string, dynamic>() {
            { "name", reader["name"] },
            { "count", reader["count"] }
        };
        await reader.CloseAsync();

        return table_data;
    }
    
    public static async void add_by_id(int id, int count)
    {Library.Database db = new Library.Database();

        var cmd = new NpgsqlCommand("SELECT id FROM items", db.Conn);

        List<int> exists = new List<int>();

        var reader = await db.Query(cmd);
        while (await reader.ReadAsync())
        {
            exists.Add((int)reader["id"]);
        }
        await reader.CloseAsync();
        
        if (exists.Contains(id))
        {
            cmd = new NpgsqlCommand("UPDATE items SET count = ($1) WHERE id = ($2) ", db.Conn)
            {
                Parameters =
                {
                    new () { Value = count},
                    new () { Value = id }
                }
            };
            db.Store(cmd);
        }
    }
}