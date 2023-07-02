namespace ModelLayer.OrderModel; 

public class OrderedItem {
    public int ID { get; set; }
    public int OrderID { get; set; }
    public int ItemID { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
}