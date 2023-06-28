using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Notification;
using Avalonia.Threading;
using GUI.Logic.Models.Table;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class TablesViewModel : RoutablePage {


  /*
   * This holds the state information
   * of the tables. This is for us to check
   * if the table that was clicked has been taken
   */
  private TablesModel Tables {
    get;
    set;
  }

  private DispatcherTimer dispatcher = new DispatcherTimer();

  private TablesService _service;

  public TablesViewModel(IHostScreen screen) {
    HostScreen = screen;
    LogoutUser = ReactiveCommand.Create(logoutUserAction);
    // TableSel = ReactiveCommand.Create<string>(tableSelect);
    MyTables = ReactiveCommand.Create(showUserTables);
    Reservations = ReactiveCommand.Create(() => { HostScreen.GoNext(new ReservationsViewModel(HostScreen)); });

    _service = new TablesService();

    Tables = new TablesModel(
        convertToContainers(_service.GetAll())
    );
    
    loadTables();

    // This allows us to update the table states every "n" seconds
    // In this case n is 1
    dispatcher.Interval = TimeSpan.FromSeconds(1);
    dispatcher.Tick += loadTables;
    dispatcher.Start();
  }
  private void loadTables(object? sender, EventArgs e) {
    // handle normally
    loadTables();
  }

  public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }
  public ReactiveCommand<Unit, Unit> Reservations { get; set; }
  public ReactiveCommand<Unit, Unit> MyTables { get; set; }
  public ReactiveCommand<string, Unit> TableSel { get; set; }


  public override IHostScreen HostScreen { get; }

  void loadTables() {
    List<Table> tables = _service.GetAll();
    
    // Set tables so that the page knows their states
    // Tables.Items.Clear();
    
    Console.WriteLine($"{Tables.Items.Count}");
    
    // setTablesColours();
    // setTablesStatuses();
  }

  List<TableContainer> convertToContainers(List<Table> tables) {
    return tables.Select(table => new TableContainer
    {
        Table = table,
        Colour = "#B5ECA1", 
        OrderStatus = "none",
        TableSelect = ReactiveCommand.Create(this.tableSelect)
    }).ToList();

  }

  void setTablesColours() {
    // Move to class later
    string empty = "#B5ECA1";
    string taken = "#F38BA8";
    string reserved = "#B4BEFE";
    
    foreach (TableContainer table in Tables.Items) {
      string propertyName = $"Colour{table.Table.Number}";
      string colourValue = table.Table.Status switch {
          TableStatus.reserved => reserved,
          TableStatus.empty => empty,
          TableStatus.occupied => taken,
      };

      // Set the property dynamically using reflection
      PropertyInfo property = GetType().GetProperty(propertyName)!;
      property.SetValue(this, colourValue);
    }
  }
  
  void setTablesStatuses() {
    foreach (TableContainer table in Tables.Items) {
      string status = "";
      string propertyName = $"Table{table.Table.Number}Status";
      PropertyInfo statusProperty = GetType().GetProperty(propertyName)!;

      // Task.Run(async () =>
      // {
      //   List<string> orders = await TableType.getOrders(table.Number);
      //   
      //   if (orders.Contains("done")) {
      //     status = "done";
      //   }
      //   else if (orders.Contains("preparing")) {
      //     status = "preparing";
      //   }
      //   else if (orders.Contains("placed")) {
      //     status = "placed";
      //   }
      //
      // }).Wait();

      statusProperty.SetValue(this, status);
    }
  }

  void showUserTables() {
    // foreach (TableType table in Tables) {
    //
    //   // Kunal is on this
    //   // Check issue at https://github.com/TeamSoos/Shampoo/issues/16
    // }
  }

  public void tableSelect() {
    // Create tables dynamically
    Console.WriteLine("Button pressed");
  }

  // This should optimaly be handled by our IHostScreen provider
  // Check issue at https://github.com/TeamSoos/Shampoo/issues/15
  public void logoutUserAction() {
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Logging out. Good bye {HostScreen.CurrentUser}!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();
    HostScreen.GoNext(new LoginPageViewModel(HostScreen));
  }

  public override void OnUnload() {
    dispatcher.Stop();
    Console.WriteLine("Dispatcher halted");
  }

  public override void OnLoad() {
    dispatcher.Start();
    Console.WriteLine("Dispatcher resumed");
  }
}


public class TableContainer {
  public ReactiveCommand<Unit, Unit> TableSelect { get; set; }
  public Table Table {
    get => table;
    set => table = value;
  }
  private Table table;
  public string Colour {
    get => colour;
    set => colour = value;
  }
  private string colour;
  public string OrderStatus {
    get => orderStatus;
    set => orderStatus = value;
  }
  private string orderStatus;
}

public class TablesModel : ViewModelBase
{
  public TablesModel(IEnumerable<TableContainer> items)
  {
    Items = new ObservableCollection<TableContainer>(items);
  }

  private ObservableCollection<TableContainer> _listItems;

  public ObservableCollection<TableContainer> Items {
    get => _listItems;
    set => this.RaiseAndSetIfChanged(ref _listItems, value);
  }
}
