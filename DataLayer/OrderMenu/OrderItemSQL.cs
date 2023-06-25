using ModelLayer.OrderMenu;
using Npgsql;

namespace DataLayer.OrderMenu;

public class OrderItemSQL : BaseSQL<OrderMenuItem> {
    public async Task<List<OrderMenuItem>> get_all() {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu");
        return await QueryMultiple(cmd);
    }

    public async Task<List<OrderMenuItem>> get_of_menu(OrderMenuItem.EMenuType menu) {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu WHERE menu = @menu");
        cmd.Parameters.AddWithValue("menu", OrderMenuItem.StringFromMenu(menu));
        return await QueryMultiple(cmd);
    }


    protected override OrderMenuItem ReadTables(NpgsqlDataReader reader) {

        // need to convert reader["type"] to an enum

        return new OrderMenuItem {
            ID = (int)reader["id"],
            Count = (int)reader["count"],
            Price = (decimal)reader["price"],
            Name = (string)reader["name"],
            Type = (string)reader["type"],
            Menu = OrderMenuItem.MenuFromString((string)reader["menu"]),
        };
    }
}