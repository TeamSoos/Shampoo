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
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;

namespace GUI.ViewModels;

public class SelectTableViewModel : RoutablePage {
  ObservableCollection<ComboBoxItem> comboBoxItems;

  public int currentTable;

  public int guestCount;

  int selected_index;
  

  public TableType Table { get; set; }


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
  public ReactiveCommand<Unit, Unit>  DeliverOrder { get; set; }
  public int UnpaidOrders { get; set; }

#pragma warning disable CS8618
  public SelectTableViewModel(IHostScreen screen) {
#pragma warning restore CS8618

    GuestCount = 0;
    HostScreen = screen;
    CurrentTable = screen.CurrentTable;
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

    GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

    // Load statuses for buttons
    this.Table = new TableType(CurrentTable);

    // State machine
    switch (Table.status) {
      case Status.reserved:
      case Status.empty:
        IsFreeable = false;
        IsOccupiable = true;
        break;
      case Status.occupied:
        IsFreeable = true;
        IsOccupiable = false;
        break;
    }

    // Load orders paid information
    Task.Run(async void () =>
    {
      UnpaidOrders = (await TableType.getOrdersTyped(CurrentTable)).Count(order => !order.Paid);
    }).Wait();

    // Initialize the ComboBoxItems collection
    ComboBoxItems = new ObservableCollection<ComboBoxItem>();
    loadWaiters();
  }
  private async void freeTable() {
    // We need to check if the table has any unpaid orders first
    if ((await TableType.getOrders(CurrentTable)).Count > 0) {
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"Table {CurrentTable} is ready to finish dinning")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(2))
          .Queue();
      // Go to payment
      // Unfortunatly need to do this here since the payment part fails to free table
      TableType.free_single(CurrentTable);
      Logger.addRecord(HostScreen.CurrentUserID, $"Freed table {CurrentTable}");
      HostScreen.GoNext(new PaymentsViewModel(HostScreen));
    }
    else {
      // Otherwise just free the table
      TableType.free_single(CurrentTable);
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"Table {CurrentTable} has been freed")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(2))
          .Queue();

      Logger.addRecord(HostScreen.CurrentUserID, $"Freed table {CurrentTable}");
      HostScreen.GoNext(new TablesViewModel(HostScreen));
    }
  }
  private void occupyTable() {
    TableType.occupy_single(CurrentTable);
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Table {CurrentTable} has been occupied")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(2))
        .Queue();

    Logger.addRecord(HostScreen.CurrentUserID, $"Occupied table {CurrentTable}");

    HostScreen.GoNext(new TablesViewModel(HostScreen));
  }

  void createOrder() {
    string Employee = (string)ComboBoxItems[SelectedIndex].Content;
    // You can get the table id like this
    int table_id = HostScreen.CurrentTable;
    // Logged in employee is always contained like this
    string current_employee = HostScreen.CurrentUser.Name;

    // Move to your page here
    Console.WriteLine($"{Employee} for {table_id} by {current_employee}");
    HostScreen.GoNext(new OrderMenuViewModel(HostScreen));
  }

  void loadWaiters() {
    // Collect all waiters
    List<EmployeeType> employees = new List<EmployeeType>();
    Task.Run(async () =>
    {
      employees = await EmployeeType.getAll("waiter");

      Console.WriteLine("Done getting waiters");
    }).Wait();

    foreach (ComboBoxItem item in employees.Select(employee => new ComboBoxItem { Content = employee.name })) {
      ComboBoxItems.Add(item);
    }
  }
}