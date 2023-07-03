using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Notification;
using GUI.Logic.Models.Employee;
using GUI.Logic.Models.Order;
using GUI.Logic.Models.Table;
using ModelLayer;
using ModelLayer.Tables;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;
using ServiceLayer.Employee;
using ServiceLayer.Order;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class SelectTableViewModel : RoutablePage {
  ObservableCollection<ComboBoxItem> comboBoxItems;

  public int currentTable;

  public int guestCount;

  int selected_index;

  public EmployeeService _employeeService;
  public TablesService _tableService;
  public OrderService _orderService;
  public Table Table { get; set; }


  public override IHostScreen HostScreen { get; }

  public ObservableCollection<ComboBoxItem> ComboBoxItems {
    get => comboBoxItems;
    set => this.RaiseAndSetIfChanged(ref comboBoxItems, value);
  }

  public int SelectedIndex {
    get => selected_index;
    set => this.RaiseAndSetIfChanged(ref selected_index, value);
  }

  public int CurrentTable {
    get => currentTable;
    set => this.RaiseAndSetIfChanged(ref currentTable, value);
  }

  public int GuestCount {
    get => guestCount;
    set => this.RaiseAndSetIfChanged(ref guestCount, value);
  }

  public ReactiveCommand<Unit, Unit> CreateOrder { get; set; }

  public ReactiveCommand<Unit, Unit> GoBack { get; }

  public ReactiveCommand<Unit, Unit> GoToReserve { get; }
  public ReactiveCommand<Unit, Unit> OccupyTable { get; }
  public ReactiveCommand<Unit, Unit> FreeTable { get; }
  public bool IsFreeable { get; set; }
  public bool IsOccupiable { get; set; }
  public ReactiveCommand<Unit, Unit> DeliverOrder { get; set; }
  public int UnpaidOrders { get; set; }

#pragma warning disable CS8618
  public SelectTableViewModel(IHostScreen screen) {
#pragma warning restore CS8618

    GuestCount = 0;
    HostScreen = screen;
    CurrentTable = screen.CurrentTable.Number;
    CreateOrder = ReactiveCommand.Create(createOrder);
    GoToReserve = ReactiveCommand.Create(() =>
    {
      HostScreen.GuestCount = GuestCount;
      HostScreen.GoNext(new ReserveTableViewModel(HostScreen));
    });

    DeliverOrder = ReactiveCommand.Create(() => { HostScreen.GoNext(new DeliverOrderViewModel(HostScreen)); });

    CreateOrder = ReactiveCommand.Create(createOrder);
    OccupyTable = ReactiveCommand.Create(occupyTable);
    FreeTable = ReactiveCommand.Create(freeTable);
    // Create table and order services
    _tableService = new();
    _orderService = new ();

    GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

    // Load statuses for buttons
    this.Table = HostScreen.CurrentTable;

    // States
    switch (Table.Status) {
      case TableStatus.reserved:
      case TableStatus.empty:
        IsFreeable = false;
        IsOccupiable = true;
        break;
      case TableStatus.occupied:
        IsFreeable = true;
        IsOccupiable = false;
        break;
    }

    // Load orders paid information
    UnpaidOrders = (_orderService.GetAllByTable(screen.CurrentTable)).Count(order => !order.Paid);

    // Initialize the ComboBoxItems collection
    ComboBoxItems = new ObservableCollection<ComboBoxItem>();
    // Start loading waiters
    _employeeService = new EmployeeService();
    loadWaiters();
  }
  private async void freeTable() {
    // We need to check if the table has any unpaid orders first
    List<Order> orders = _orderService.GetAllByTable(HostScreen.CurrentTable)
        .Where(order => !order.Paid)
        .ToList();

    foreach (var oder in orders) {
      Console.WriteLine($"{oder.ID} {oder.Paid}");
    }
    if (orders.Count > 0) {
      HostScreen.Notify($"Table {CurrentTable} is ready to finish dinning", 2);
      // Go to payment
      Logger.addRecord(HostScreen.CurrentUserID, $"Freed table {CurrentTable}");
      HostScreen.GoNext(new PaymentsViewModel(HostScreen));
    }
    else {
      _tableService.Free(HostScreen.CurrentTable);

      Logger.addRecord(HostScreen.CurrentUserID, $"Freed table {CurrentTable}");
      HostScreen.GoNext(new TablesViewModel(HostScreen));
    }
  }
  private void occupyTable() {
    _tableService.Occupy(HostScreen.CurrentTable);

    Logger.addRecord(HostScreen.CurrentUserID, $"Occupied table {CurrentTable}");

    HostScreen.GoNext(new TablesViewModel(HostScreen));
  }

  void createOrder() {
    HostScreen.GoNext(new OrderMenuViewModel(HostScreen));
  }

  void loadWaiters() {
    // Collect all waiters
    List<Employee> employees = new List<Employee>();
    employees = _employeeService.GetAllByJob(EmployeeJob.waiter);

    foreach (ComboBoxItem item in employees.Select(employee => new ComboBoxItem { Content = employee.Name })) {
      ComboBoxItems.Add(item);
    }
  }
}