using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Menu;

public enum EMenuType {
    Lunch,
    Dinner,
    Drinks,
}

public class MenuSql {
    public static async Task<List<MenuType>> get_all(EMenuType type) {
        Library.Database db = new Library.Database();
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;

        var items = new List<MenuType>();

        cmd = type switch {
            EMenuType.Lunch => new NpgsqlCommand("SELECT * FROM lunchmenu", db.Conn),
            EMenuType.Dinner => new NpgsqlCommand("SELECT * FROM dinnermenu", db.Conn),
            EMenuType.Drinks => new NpgsqlCommand("SELECT * FROM drinkmenu", db.Conn),
            _ => throw new Exception("Unreachable code reached. Good job.")
        };

        reader = await db.Query(cmd);

        while (reader.Read()) {
            MenuType item = new MenuType((int)reader["id"], (string)reader["name"], (string)reader["type"],
                (decimal)reader["price"]);
            items.Add(item);
        }

        return items;
    }
}