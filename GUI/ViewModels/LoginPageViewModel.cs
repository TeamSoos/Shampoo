using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Notification;
using DynamicData.Tests;
using GUI.Views;
using Logic.SQL;
using Npgsql;
using ReactiveUI;
using RoutedApp.Logic;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class LoginPageViewModel : RoutablePage {
  private string _text;

  public string Text
  {
    get => _text;
    set => _text = value;
  }

  private List<string> staff = new List<string>() { "chef", "barman", "waiter"};

  public ICommand LoginStaff { get; }

  private bool _mobileui; 
  
  public bool MobileUIRequested {
    get => _mobileui;
    set => this.RaiseAndSetIfChanged(ref _mobileui, value);
  }

  public LoginPageViewModel(IHostScreen screen) {
    MobileUIRequested = false;
    HostScreen = screen;
    LoginStaff = ReactiveCommand.Create(loginStaff);
  }
  
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING

  private async Task loginStaff() {
    Console.WriteLine($"Login key: {Text}");
    Library.Database db = new Library.Database();

    var cmd = new NpgsqlCommand("SELECT id, job, name FROM employees WHERE login=($1)", db.Conn)
    {
        Parameters =
        {
            new() { Value = Text }
        }
    };
    var reader = await db.Query(cmd);


    while (await reader.ReadAsync()) {
      if (reader["id"] != null) // Ignore the warning, stupid C#
      {
        // We have an ID, let's check if it's one of the staff
        if (staff.Contains(reader["job"].ToString()!)) {
          Console.WriteLine($"Staff access! {MobileUIRequested} {reader["job"]} {reader["name"]}");

          if (MobileUIRequested) {
            Console.WriteLine("User requested mobile interface");
            // end me...
            UIController ncontroller = UIController.GetInstance(null);
            ncontroller.ResizeWindow(400, 800);
          }

          HostScreen.mobileUI = MobileUIRequested;

          HostScreen.CurrentUser = reader["name"].ToString()!;
          
          HostScreen.notificationManager.CreateMessage()
              .Animates(true)
              .Background("#B4BEFE")
              .Foreground("#1E1E2E")
              .HasMessage(
                  $"Welcome back {reader["name"]}")
              .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
              .Queue();

          switch (reader["job"].ToString()!) {
            case "chef":
              HostScreen.GoNext(new OrderingViewModel(HostScreen));
              return;
            case "waiter":
              Console.WriteLine("Waiter");
              HostScreen.GoNext(new TablesViewModel(HostScreen));
              return;
          }

        }
        else {
          Console.WriteLine("Not staff!");
        }
      }
    }
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            "Error: Invalid credentials")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();

  }
  public override IHostScreen HostScreen { get; }
}