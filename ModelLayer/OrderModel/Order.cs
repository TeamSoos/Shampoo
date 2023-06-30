using ModelLayer.Tables;

namespace ModelLayer; 

public class Order {
  public int ID { get; set; }
  public Table Table { get; set; }
  public bool Paid { get; set; }
  public string Status { get; set; }
}