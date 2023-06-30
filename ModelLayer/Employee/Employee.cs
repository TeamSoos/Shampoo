namespace ModelLayer; 

public class Employee {
  public int ID { get; set; }
  public EmployeeJob Job { get; set; }
  public string Name { get; set; }
  public EmployeeLogin Login { get; set; }
}