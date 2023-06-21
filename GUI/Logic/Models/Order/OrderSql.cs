using System;
using System.Collections.Generic;
using System.Linq;
using GUI.Logic.Models.Menu;
using GUI.ViewModels;
using Logic.SQL;
using Npgsql;

namespace GUI.Logic.Models.Order;

public class OrderSql {
    public static async void place_order(List<MenuItemGrouped> items, int table_id) {
        Library.Database db = new Library.Database();
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;

        cmd = new NpgsqlCommand("INSERT INTO orders (id, table_id) VALUES (DEFAULT, @a) RETURNING id", db.Conn);
        cmd.Parameters.AddWithValue("a", table_id);
        reader = await db.Query(cmd);

        reader.Read();
        int order_id = (int)reader["id"];

        await reader.CloseAsync();

        foreach (var item in items) {
            var menu_item = item.item;

            var insrt_cmd =
                new NpgsqlCommand("INSERT INTO ordered_items (order_id, item_id, quantity, note) VALUES (@a, @b, @c, @d)",
                    db.Conn);
            insrt_cmd.Parameters.AddWithValue("a", order_id);
            insrt_cmd.Parameters.AddWithValue("b", menu_item.ID);
            insrt_cmd.Parameters.AddWithValue("c", item.Quantity);
            insrt_cmd.Parameters.AddWithValue("d", item.Note);
            
            Console.WriteLine(item.Note);

            // we now need to decrement the item count
            MenuSql.decrement_count(menu_item.ID, item.Quantity);

            var r = await db.Query(insrt_cmd);
            r.Close();
        }

        db.Finalize();
    }
    public static void deliver_by_id(int id) {
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd =
                new NpgsqlCommand(
                        "UPDATE orders SET status = 'none' WHERE id = ($1);",
                        db.Conn) {
                        Parameters = {
                                new NpgsqlParameter { Value = id },
                        }
                };

        db.Store(cmd);
    }
}