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
        var cmd = new NpgsqlCommand("SELECT SUM(price) AS total_price FROM orders JOIN ordered_items oi ON orders.id = oi.order_id JOIN allmenu a ON oi.item_id = a.id WHERE table_id = @a AND paid = false;");
        cmd.Parameters.AddWithValue("@a", tableid);
        var result = await QueryOne(cmd);
        return result;
    }
    
    public void CreatePayment(PaymentModel payment) // wait for kunal sql update string
    {
        var cmd = new NpgsqlCommand("INSERT INTO payments (amount, payment_date) VALUES (@amount, @paymentDate) RETURNING payment_id;");
        cmd.Parameters.AddWithValue("@amount", payment.TotalAmount);
        cmd.Parameters.AddWithValue("@paymentDate", payment.PaymentDate);

        Store(cmd);
    }
    
    protected override PaymentModel ReadTables(NpgsqlDataReader reader) // converts internal reader type into my model
    {
        return new PaymentModel()
        {
            TotalAmount = (decimal)reader["total_price"]
        };
    }
}