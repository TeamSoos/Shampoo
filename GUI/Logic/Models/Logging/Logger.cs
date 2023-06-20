namespace RoutedApp.Logic.Models.Logging; 

public class Logger {
  public static void addRecord(int employee, string action) {
    LoggerSQL.addRecord(employee, action);
  }
}