using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Notification;
using GUI.Views;
using Logic.SQL;
using Npgsql;
using ReactiveUI;
using RoutedApp.Logic;

namespace GUI.ViewModels;

public class LoginPageViewModel : RoutablePage {

    bool _mobileui;

    readonly List<string> staff = new List<string> { "admin", "chef", "barman", "waiter" };

    public LoginPageViewModel(IHostScreen screen) {
        MobileUIRequested = false;
        HostScreen = screen;
        LoginStaff = ReactiveCommand.Create(loginStaffTask);
    }

    public string Text { get; set; }

    public ICommand LoginStaff { get; }

    public bool MobileUIRequested {
        get => _mobileui;
        set => this.RaiseAndSetIfChanged(ref _mobileui, value);
    }

    // Styling
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;

    public override IHostScreen HostScreen { get; }
    // END STYLING

    async Task loginStaffTask() {
        Console.WriteLine($"Login key: {Text}");
        Library.Database db = new Library.Database();

        NpgsqlCommand cmd = new NpgsqlCommand("SELECT id, job, name FROM employees WHERE login=($1)", db.Conn) {
            Parameters = {
                new NpgsqlParameter { Value = Text }
            }
        };
        NpgsqlDataReader reader = await db.Query(cmd);


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
                        case "admin":
                            Console.WriteLine("Admin");
                            HostScreen.GoNext(new NewPageViewModel(HostScreen));
                            return;
                    }

                } else {
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
}