namespace ModelLayer.Tables; 

public class Reservation {
  public int ID { get; set; }
  public int Guests {get; set;}
  public string Name {get; set;}
  public string Phone {get; set;}
  public Table Table {get; set;}
  public DateTime Time {get; set;}
}