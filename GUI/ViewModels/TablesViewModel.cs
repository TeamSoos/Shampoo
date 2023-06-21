using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Notification;
using Avalonia.Threading;
using GUI.Logic.Models.Table;
using ReactiveUI;

namespace GUI.ViewModels;

public class TablesViewModel : RoutablePage {


  /*
   * This basically holds the state information
   * of the tables. This is for us to check
   * if the table that was clicked has been taken
   */

  List<TableType> Tables;

  /*
   * REDUNDANT
   * This is for the function TableSel
   * to wait until all the table
   * states have been loaded
   */
  // public bool tablesLoaded = false;


  private DispatcherTimer dispatcher = new DispatcherTimer();

  public TablesViewModel(IHostScreen screen) {
    HostScreen = screen;
    LogoutUser = ReactiveCommand.Create(logoutUserAction);
    TableSel = ReactiveCommand.Create<string>(tableSelect);
    MyTables = ReactiveCommand.Create(showUserTables);
    Reservations = ReactiveCommand.Create(() => { HostScreen.GoNext(new ReservationsViewModel(HostScreen)); });


    // We now want to inform the buttons of their states
    // This takes a while to load, need to offload it somehow
    // I think if I remove the wait, it starts to work fine
    // nvm can't async set the colours of the tables since threads-
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
    Console.WriteLine("Starting to set tables");
    List<TableType> tables = new List<TableType>();
    Task.Run(async () =>
    {
      // Populate table state
      tables = await TableType.getAll();

    }).Wait();
    Tables = tables;

    string empty = "#B5ECA1";
    string taken = "#F38BA8";
    string reserved = "#B4BEFE";

    foreach (TableType table in tables) {
      string propertyName = $"Colour{table.number}";
      string colourValue = table.status switch {
          Status.reserved => reserved,
          Status.empty => empty,
          Status.occupied => taken,
          _ => throw new ArgumentOutOfRangeException()
      };

      // Set the property dynamically using reflection
      PropertyInfo property = GetType().GetProperty(propertyName)!;
      property.SetValue(this, colourValue);


      // Do the same for order status

      string status = "";
      propertyName = $"Table{table.number}Status";
      PropertyInfo statusProperty = GetType().GetProperty(propertyName)!;

      Task.Run(async () =>
      {
        List<string> orders = await TableType.getOrders(table.number);
        
        if (orders.Contains("done")) {
          status = "done";
        }
        else if (orders.Contains("preparing")) {
          status = "preparing";
        }
        else if (orders.Contains("placed")) {
          status = "placed";
        }

      }).Wait();

      statusProperty.SetValue(this, status);
    }

    Console.WriteLine("Done setting states");
  }

  void showUserTables() {
    foreach (TableType table in Tables) {

      // Kunal is on this
      // Check issue at https://github.com/TeamSoos/Shampoo/issues/16
    }
  }

  // Helper func -  Redundant?
  // bool IsNumeric(string content) {
  //     return int.TryParse(content, out _);
  // }

  public async void tableSelect(string button) {
    Console.WriteLine($"Button pressed: {button}");

    int button_id = int.Parse(button);

    // This is ugly and slow, could be made into a hashmap
    /* Loop over every table in known tables and
     * ensure that the table is interactable
     * Notify the user if it's reserved or occupied
     */
    foreach (TableType table in Tables) {
      if (table.number == button_id && table.status == Status.occupied) {
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Notice! This table is currently occupied")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(2))
            .Queue();
      }
      else if (table.number == button_id && table.status == Status.reserved) {
        DateTime time = await TableSQL.get_reservation_time(table.number);
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Notice! This table is reserved for someone at [{time:hh:mm}]")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(2))
            .Queue();
      }
    }

    HostScreen.CurrentTable = button_id;

    HostScreen.GoNext(new SelectTableViewModel(HostScreen));
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

  // (𓁹󠁘◡𓁹) good luck~ 
  // Yeah Im not going to try and cause a memory leak by
  // creating these during runtime. CBA
  //
  // Kunal if you have free time, you can try your hand at this

  string colour1;

  string colour10;

  string colour2;

  string colour3;

  string colour4;

  string colour5;

  string colour6;

  string colour7;

  string colour8;

  string colour9;
  public string Colour1 {
    get => colour1;
    set => this.RaiseAndSetIfChanged(ref colour1, value);
  }

  public string Colour2 {
    get => colour2;
    set => this.RaiseAndSetIfChanged(ref colour2, value);
  }

  public string Colour3 {
    get => colour3;
    set => this.RaiseAndSetIfChanged(ref colour3, value);
  }

  public string Colour4 {
    get => colour4;
    set => this.RaiseAndSetIfChanged(ref colour4, value);
  }

  public string Colour5 {
    get => colour5;
    set => this.RaiseAndSetIfChanged(ref colour5, value);
  }

  public string Colour6 {
    get => colour6;
    set => this.RaiseAndSetIfChanged(ref colour6, value);
  }

  public string Colour7 {
    get => colour7;
    set => this.RaiseAndSetIfChanged(ref colour7, value);
  }

  public string Colour8 {
    get => colour8;
    set => this.RaiseAndSetIfChanged(ref colour8, value);
  }

  public string Colour9 {
    get => colour9;
    set => this.RaiseAndSetIfChanged(ref colour9, value);
  }

  public string Colour10 {
    get => colour10;
    set => this.RaiseAndSetIfChanged(ref colour10, value);
  }
  private string table1Status;
  public string Table1Status {
    get => table1Status;
    set => this.RaiseAndSetIfChanged(ref table1Status, value);
  }
  private string table2Status;
  public string Table2Status {
    get => table2Status;
    set => this.RaiseAndSetIfChanged(ref table2Status, value);
  }

  private string table3Status;
  public string Table3Status {
    get => table3Status;
    set => this.RaiseAndSetIfChanged(ref table3Status, value);
  }

  private string table4Status;
  public string Table4Status {
    get => table4Status;
    set => this.RaiseAndSetIfChanged(ref table4Status, value);
  }

  private string table5Status;
  public string Table5Status {
    get => table5Status;
    set => this.RaiseAndSetIfChanged(ref table5Status, value);
  }

  private string table6Status;
  public string Table6Status {
    get => table6Status;
    set => this.RaiseAndSetIfChanged(ref table6Status, value);
  }

  private string table7Status;
  public string Table7Status {
    get => table7Status;
    set => this.RaiseAndSetIfChanged(ref table7Status, value);
  }

  private string table8Status;
  public string Table8Status {
    get => table8Status;
    set => this.RaiseAndSetIfChanged(ref table8Status, value);
  }

  private string table9Status;
  public string Table9Status {
    get => table9Status;
    set => this.RaiseAndSetIfChanged(ref table9Status, value);
  }

  private string table10Status;
  public string Table10Status {
    get => table10Status;
    set => this.RaiseAndSetIfChanged(ref table10Status, value);
  }
}