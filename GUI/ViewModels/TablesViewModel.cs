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
using DynamicData;
using GUI.Logic.Models.Table;
using ModelLayer;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Order;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class TablesViewModel : RoutablePage {


  /*
   * This holds the state information
   * of the tables. This is for us to check
   * if the table that was clicked has been taken
   */
  private TablesModel _tables;
  public TablesModel Tables  {
    get => _tables;
    set => this.RaiseAndSetIfChanged(ref _tables, value);
  }

  private DispatcherTimer dispatcher = new DispatcherTimer();

  private TablesService _tableService;
  private OrderService _orderService;

  public TablesViewModel(IHostScreen screen) {
    HostScreen = screen;
    LogoutUser = ReactiveCommand.Create(screen.LogoutUserAction);
    MyTables = ReactiveCommand.Create(showUserTables);
    Reservations = ReactiveCommand.Create(() => { HostScreen.GoNext(new ReservationsViewModel(HostScreen)); });

    _tableService = new TablesService();
    _orderService = new OrderService();

    Tables = new TablesModel(
        convertToContainers(_tableService.GetAll())
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


  public override IHostScreen HostScreen { get; }

  void loadTables() {
    List<Table> tables = _tableService.GetAll();
    List<TableContainer> containers = convertToContainers(tables);

    Console.WriteLine($"Tables: {Tables.Items.Count}");
    
    // Set tables so that the page knows their states
    Tables = new TablesModel(
        containers
    );
    
    // We use a global value since the model is
    // regarded as a reactive element
    setTablesColours();
    setTablesStatuses();
  }

  List<TableContainer> convertToContainers(List<Table> tables) {
    return tables.Select(table => new TableContainer
    {
        Table = table,
        Colour = "#B5ECA1", 
        OrderStatus = "orderStatus",
        TableSelect = ReactiveCommand.Create<Int32, Unit>(this.tableSelect)
    }).ToList();

  }

  void setTablesColours() {
    // Move to class later
    string empty = "#B5ECA1";
    string taken = "#F38BA8";
    string reserved = "#B4BEFE";
    
    foreach (TableContainer table in Tables.Items) {
      Console.WriteLine($"{table.Table.Number} {table.Table.Status}");
      string colourValue = table.Table.Status switch {
          TableStatus.reserved => reserved,
          TableStatus.empty => empty,
          TableStatus.occupied => taken,
      };

      table.Colour = colourValue;
    }
  }
  
  void setTablesStatuses() {
    foreach (TableContainer table in Tables.Items) {
      string status = "";
      List<Order> orders = _orderService.GetAllByTable(table.Table);

      foreach (var order in orders) {
        if (order.Paid)
          continue;

        if (order.Status == "done") {
          status = "done";
        } else if (order.Status == "placed") {
          status = "placed";
        }
      }
      
      table.OrderStatus = status;
    }
  }

  void showUserTables() {
    // foreach (TableType table in Tables) {
    //
    //   // Kunal is on this
    //   // Check issue at https://github.com/TeamSoos/Shampoo/issues/16
    // }
  }

  public Unit tableSelect(Int32 x) {
    Console.WriteLine("Button pressed");
    // Load next
    HostScreen.CurrentTable = x;
    HostScreen.GoNext(new SelectTableViewModel(HostScreen));
    // return val bypass
    return new Unit();
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

public class TableContainer : ViewModelBase {
  public ReactiveCommand<Int32, Unit> TableSelect { get; set; }
  public Table Table {
    get => table;
    set => this.RaiseAndSetIfChanged(ref table, value);
  }
  private Table table;
  public string Colour {
    get => colour;
    set => this.RaiseAndSetIfChanged(ref colour, value);
  }
  private string colour;
  public string OrderStatus {
    get => orderStatus;
    set => this.RaiseAndSetIfChanged(ref orderStatus, value);
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
