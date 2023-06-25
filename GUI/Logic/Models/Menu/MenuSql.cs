using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using ModelLayer.OrderMenu;
using Npgsql;

namespace GUI.Logic.Models.Menu;

class MenuSql {
    public static void decrement_count(int id, int count) {
        Library.Database db = new Library.Database();
        
        NpgsqlCommand cmd = new NpgsqlCommand("UPDATE allmenu SET count = count - @a WHERE id = @b", db.Conn);
        cmd.Parameters.AddWithValue("a", count);
        cmd.Parameters.AddWithValue("b", id);
        
        db.Store(cmd);
    }
    
    public static async Task<List<MenuType>> get_all(OrderMenuItem.EMenuType type) {
        Library.Database db = new Library.Database();
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;

        var items = new List<MenuType>();

        cmd = type switch {
            OrderMenuItem.EMenuType.Lunch => new NpgsqlCommand(
                "SELECT id, name, type, price, count FROM allmenu WHERE menu = 'lunch'",
                db.Conn),
            OrderMenuItem.EMenuType.Dinner => new NpgsqlCommand(
                "SELECT id, name, type, price, count FROM allmenu WHERE menu = 'dinner'",
                db.Conn),
            OrderMenuItem.EMenuType.Drinks => new NpgsqlCommand(
                "SELECT id, name, type, price, count FROM allmenu WHERE menu = 'drinks'",
                db.Conn),
            _ => throw new Exception("Unreachable code reached. Good job.")
        };

        reader = await db.Query(cmd);

        while (reader.Read()) {
            MenuType item = new MenuType((int)reader["id"], (string)reader["name"], (string)reader["type"],
                (decimal)reader["price"], (int)reader["count"]);
            items.Add(item);
        }
        

        return items;
    }
}