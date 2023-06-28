using DataLayer.Payments;
using ModelLayer.Payment;

namespace ServiceLayer.Payment;

public class PaymentService
{
    private PaymentSQL sql = new();
    public PaymentModel GetTotalPrice(int table)
    {
        PaymentModel? result = null;
        // we make this async function wait the result 
        Task.Run(async () =>
        {
          result =  await sql.get_by_id(table);
        }).Wait();
        return result!;
    }
}