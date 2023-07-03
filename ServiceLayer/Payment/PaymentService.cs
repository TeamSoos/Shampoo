using DataLayer.Payments;
using ModelLayer.Payment;
using ModelLayer.Tables;

namespace ServiceLayer.Payment;

public class PaymentService
{
    private PaymentSQL sql = new();
    public PaymentModel GetTotalPrice(Table table)
    {
        PaymentModel? result = null;
        // we make this async function wait the result 
        Task.Run(async () =>
        {
          result =  await sql.get_by_id(table.ID);
        }).Wait();
        return result!;
    }

    public void Create(PaymentModel payment)
    {
        sql.CreatePayment(payment);
    }
}
