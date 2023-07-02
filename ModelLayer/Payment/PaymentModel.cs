namespace ModelLayer.Payment;

public class PaymentModel
{
    public int ID { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public int NumberOfPeople { get; set; } = 1; // default value
    
    public int OrderID { get; set; }
    
    public DateTime PaymentDate { get; set; }
}