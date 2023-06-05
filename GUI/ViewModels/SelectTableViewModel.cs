using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using GUI.Logic.Models.Employee;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class SelectTableViewModel : RoutablePage {

  public override IHostScreen HostScreen { get; }
  private ObservableCollection<ComboBoxItem> comboBoxItems;

  public ObservableCollection<ComboBoxItem> ComboBoxItems {
    get { return comboBoxItems; }
    set { this.RaiseAndSetIfChanged(ref comboBoxItems, value); }
  }
  private int selected_index;
  public int SelectedIndex {
    get { return selected_index; }
    set { this.RaiseAndSetIfChanged(ref selected_index, value); }
  }
  public int currentTable;

  public int CurrentTable {
    get { return currentTable; }
    set { this.RaiseAndSetIfChanged(ref currentTable, value); }
  } 
  
  public ReactiveCommand<Unit, Unit> CreateOrder { get; set; }

#pragma warning disable CS8618
  public SelectTableViewModel(IHostScreen screen) {
#pragma warning restore CS8618
    
    HostScreen = screen;
    CurrentTable = screen.CurrentTable;
    CreateOrder = ReactiveCommand.Create(createOrder);

    // Initialize the ComboBoxItems collection
    ComboBoxItems = new ObservableCollection<ComboBoxItem>();
    loadWaiters();
  }

  void createOrder() {
    string Employee = (string)ComboBoxItems[SelectedIndex].Content;
    // You can get the table id like this
    int table_id = HostScreen.CurrentTable;
    // Logged in employee is always contained like this
    string current_employee = HostScreen.CurrentUser;

    // Move to your page here
    Console.WriteLine($"{Employee} for {table_id} by {current_employee}");
  }

  void loadWaiters() {
    // Collect all waiters
    List<EmployeeType> employees = new List<EmployeeType>();
    Task.Run(async () =>
    {
      employees = await EmployeeType.getAll("waiter");

      Console.WriteLine("Done getting waiters");
    }).Wait();

    foreach (var employee in employees) {
      ComboBoxItem item = new ComboBoxItem { Content = employee.name };
      ComboBoxItems.Add(item);
    }
  }
}