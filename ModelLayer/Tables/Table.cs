namespace ModelLayer.Tables;

public class Table {
  public int ID;
  public int Number;
  public TableStatus Status;
  public int Employee;
  
  public Table(int id, int number, TableStatus status, int employee) {
    this.ID = id;
    this.Number = number;
    this.Status = status;
    this.Employee = employee;
  }
  
  // Empty constructor for sql utility
  public Table() {}
}