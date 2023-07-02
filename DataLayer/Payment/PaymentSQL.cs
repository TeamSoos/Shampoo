using Npgsql;
using ModelLayer;
using ModelLayer.Payment;

namespace DataLayer.Payments;

public class PaymentSQL : BaseSQL<PaymentModel>
{
    public async Task<List<PaymentModel>> get_all()
    {
        var cmd = new NpgsqlCommand("SELECT SUM(price) AS total_price FROM orders JOIN ordered_items oi ON orders.id = oi.order_id JOIN allmenu a ON oi.item_id = a.id WHERE table_id = @a AND paid = false;");
        return await QueryMultipleAsync(cmd);
    }
    
    public async Task<PaymentModel> get_by_id(int tableid)
    {
        // -1 as id because here we aggregrate an infinite amount theoratically
        var cmd = new NpgsqlCommand("SELECT -1 AS id, SUM(price) AS total_price FROM orders JOIN ordered_items oi ON orders.id = oi.order_id JOIN allmenu a ON oi.item_id = a.id WHERE table_id = @a AND paid = false;");
        cmd.Parameters.AddWithValue("@a", tableid);
        var result = await QueryOne(cmd);
        return result;
    }
    
    public void CreatePayment(PaymentModel payment) // wait for kunal sql update string
    {
        var cmd = new NpgsqlCommand("INSERT INTO transactions (total) VALUES (@amount) RETURNING id, total AS total_price;");
        cmd.Parameters.AddWithValue("@amount", payment.TotalAmount);

        Store(cmd);
    }
    
    protected override PaymentModel ReadTables(NpgsqlDataReader reader) // converts internal reader type into my model
    {
        var value = (decimal?)reader["total_price"];

        if (value == null) {
            value = 0;
        }
        
        return new PaymentModel()
        {
            ID = (int)reader["id"],
            TotalAmount = (decimal)value,
        };
    }
}