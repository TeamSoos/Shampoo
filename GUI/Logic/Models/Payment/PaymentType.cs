using Logic.Models.Base;

namespace RoutedApp.Logic.Models.Payment;

public class PaymentType : BaseType
{
    
    
    
    public PaymentType(int id) : base(id)
    {
    }

    public override T getByID<T>(int id)
    {
        throw new System.NotImplementedException();
    }
}