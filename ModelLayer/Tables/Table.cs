namespace ModelLayer.Tables;

public class Table {
  public int ID { get; set; }
  public int Number { get; set; }
  public TableStatus Status { get; set; }
  public Employee Employee { get; set; }
}