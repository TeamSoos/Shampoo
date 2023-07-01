using ModelLayer.OrderMenu;
using Npgsql;

namespace DataLayer.OrderMenu;

public class OrderItemSQL : BaseSQL<OrderMenuItemModel> {
    public async Task<List<OrderMenuItemModel>> get_all() {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu");
        return await QueryMultiple(cmd);
    }

    public async Task<List<OrderMenuItemModel>> get_of_menu(OrderMenuItemModel.EMenuType menu) {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu WHERE menu = @menu");
        cmd.Parameters.AddWithValue("menu", OrderMenuItemModel.StringFromMenu(menu));
        return await QueryMultipleAsync(cmd);
    }


    protected override OrderMenuItemModel ReadTables(NpgsqlDataReader reader) {

        // need to convert reader["type"] to an enum

        return new OrderMenuItemModel {
            ID = (int)reader["id"],
            Count = (int)reader["count"],
            Price = (decimal)reader["price"],
            Name = (string)reader["name"],
            Type = (string)reader["type"],
            Menu = OrderMenuItemModel.MenuFromString((string)reader["menu"]),
        };
    }
}