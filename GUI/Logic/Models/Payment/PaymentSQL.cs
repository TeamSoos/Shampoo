using System.Threading.Tasks;
using Logic.SQL;
using Npgsql;

namespace RoutedApp.Logic.Models.Payment;

public class PaymentSQL
{
    public static async Task<decimal> get_total(int table)
    {
        Library.Database db = new Library.Database();
        var cmd = new NpgsqlCommand(
            "SELECT SUM(price) AS total_price FROM orders JOIN ordered_items oi ON orders.id = oi.order_id JOIN allmenu a ON oi.item_id = a.id WHERE table_id = @a AND paid = false;", db.Conn);

        cmd.Parameters.AddWithValue("a", table);

        var reader = await db.Query(cmd);

        await reader.ReadAsync();
        decimal price = (decimal)reader["total_price"];
        await reader.CloseAsync();
        return price;
    }
 }