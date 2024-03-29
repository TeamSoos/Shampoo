using ModelLayer.OrderMenu;
using Npgsql;

namespace DataLayer.OrderMenu;

public class OrderItemSQL : BaseSQL<OrderMenuItemModel> {
    public List<OrderMenuItemModel> get_all() {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu");
        return QueryMultiple(cmd);
    }

    public List<OrderMenuItemModel> get_of_menu(OrderMenuItemModel.EMenuType menu) {
        var cmd = new NpgsqlCommand("SELECT * FROM allmenu WHERE menu = @menu");
        cmd.Parameters.AddWithValue("menu", OrderMenuItemModel.StringFromMenu(menu));
        return QueryMultiple(cmd);
    }

    public void add_stock(OrderMenuItemModel item, int stock) {
        NpgsqlCommand cmd = new NpgsqlCommand("UPDATE allmenu SET count = count + ($1) WHERE id = ($2) ")
        {
            Parameters =
            {
                new () { Value = stock},
                new () { Value = item.ID }
            }
        };
    }

    public OrderMenuItemModel get_by_id(int id)
    {
        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM allmenu WHERE id = ($1)")
        {
            Parameters =
            {
                new () { Value = id }
            }
        };

        return QueryOneSync(cmd);
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
    
    public async void add_to_menu(OrderMenuItemModel item)
    {
        var cmd = new NpgsqlCommand("INSERT INTO allmenu (type, name, price, menu, is_alcholic, count) VALUES ($1, $2, $3, $4, $5, $6)"){
            Parameters = {
                new NpgsqlParameter { Value = item.Type },
                new NpgsqlParameter { Value = item.Name },
                new NpgsqlParameter { Value = item.Price },
                new NpgsqlParameter { Value = item.Menu.ToString().ToLower() },
                new NpgsqlParameter { Value = item.Alcoholic },
                new NpgsqlParameter { Value = item.Count },
            }
        };
        Store(cmd);
    }
}