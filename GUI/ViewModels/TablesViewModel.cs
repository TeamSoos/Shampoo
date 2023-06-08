using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Reactive;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Notification;
using ExCSS;
using GUI.Logic.Models.Table;
using GUI.Views;
using ReactiveUI;
using RoutedApp.ViewModels;
using Svg;

namespace GUI.ViewModels;

public class TablesViewModel : RoutablePage {
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  
  /*
   * This is for the function TableSel
   * to wait until all the table
   * states have been loaded
   */
  public bool tablesLoaded = false;
  
  public TablesViewModel(IHostScreen screen) {
    LogoutUser = ReactiveCommand.Create(logoutUser);
    TableSel = ReactiveCommand.Create<string>(tableSelect);
    HostScreen = screen;
    
    // We now want to inform the buttons of their states
    // F*ck this takes a while to load, need to offload it somehow
    // I think if I remove the wait, it starts to work fine
    // nvm can't async set the colours of the tables
    loadTables();
  }

  void loadTables() {
    List<TableType> tables = new List<TableType>();
    Task.Run(async () =>
    {
      // Populate table state
      tables = await TableType.getAll();
      Console.WriteLine("Done setting states");
    }).Wait();
    
    string empty = "#B5ECA1";
    string taken = "#F38BA8";
    string reserved = "#B4BEFE";

    foreach (var table in tables) {
      Console.WriteLine($"Processing table {table.number} {table.status}");
      string propertyName = $"Colour{table.number}";
      string colourValue = table.status switch
      {
          Status.reserved => reserved,
          Status.empty => empty,
          Status.occupied => taken,
          _ => throw new ArgumentOutOfRangeException()
      };

      // Set the property dynamically using reflection
      PropertyInfo property = GetType().GetProperty(propertyName)!;
      property.SetValue(this, colourValue);
        
      Console.WriteLine($"Set to {colourValue}");
      // switch (table.number) {
      //   case 1:
      //     Colour1 = table.status switch {
      //         Status.reserved => reserved,
      //         Status.empty => empty,
      //         Status.taken => taken,
      //         _ => throw new ArgumentOutOfRangeException()
      //     };
      //     break;
    }
  }

  // Helper class
  bool IsNumeric(string content)
  {
    return int.TryParse(content, out _);
  }
  public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }
  public ReactiveCommand<string, Unit> TableSel { get; set; }

  public void tableSelect(string button) {
    Console.WriteLine($"Button pressed: {button}");

    int button_id = int.Parse(button);

    HostScreen.CurrentTable = button_id;
    
    HostScreen.GoNext(new SelectTableViewModel(HostScreen));
  }

  public void logoutUser() {
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Logging out. Good bye {HostScreen.CurrentUser}")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();
    HostScreen.GoNext(new LoginPageViewModel(HostScreen));
  }

  public override IHostScreen HostScreen { get; }

  // (𓁹󠁘◡𓁹) good luck~ 
  // Yeah Im not going to try and cause a memory leak by
  // creating these during runtime. CBA
  public string Colour1 {
    get => colour1;
    set { this.RaiseAndSetIfChanged(ref colour1, value); }
  }
  private string colour1;

  public string Colour2 {
    get => colour2;
    set { this.RaiseAndSetIfChanged(ref colour2, value); }
  }
  private string colour2;

  public string Colour3 {
    get => colour3;
    set { this.RaiseAndSetIfChanged(ref colour3, value); }
  }
  private string colour3;

  public string Colour4 {
    get => colour4;
    set { this.RaiseAndSetIfChanged(ref colour4, value); }
  }
  private string colour4;

  public string Colour5 {
    get => colour5;
    set { this.RaiseAndSetIfChanged(ref colour5, value); }
  }
  private string colour5;

  public string Colour6 {
    get => colour6;
    set { this.RaiseAndSetIfChanged(ref colour6, value); }
  }
  private string colour6;

  public string Colour7 {
    get => colour7;
    set { this.RaiseAndSetIfChanged(ref colour7, value); }
  }
  private string colour7;

  public string Colour8 {
    get => colour8;
    set { this.RaiseAndSetIfChanged(ref colour8, value); }
  }
  private string colour8;

  public string Colour9 {
    get => colour9;
    set { this.RaiseAndSetIfChanged(ref colour9, value); }
  }
  private string colour9;

  public string Colour10 {
    get => colour10;
    set { this.RaiseAndSetIfChanged(ref colour10, value); }
  }
  private string colour10;
}