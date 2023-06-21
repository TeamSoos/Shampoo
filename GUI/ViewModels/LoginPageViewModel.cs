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
using static BCrypt.Net.BCrypt;

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
    public string IDInput { get; set; }

    public ICommand LoginStaff { get; }

    public bool MobileUIRequested {
        get => _mobileui;
        set => this.RaiseAndSetIfChanged(ref _mobileui, value);
    }

    // Styling
    // This is hardcoded for whatever reason, Ask Kunal
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;

    public override IHostScreen HostScreen { get; }

    async Task loginStaffTask() {
        Console.WriteLine($"Login key: {Text}");

        int.TryParse(IDInput, out int id);
        Console.WriteLine($"id: {id}");

        if (id == 0) {
            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            "Error: Invalid ID")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
        }
        
        Library.Database db = new Library.Database();

        
        var cmd = new NpgsqlCommand("SELECT job, name, login FROM employees WHERE id=($1)", db.Conn) {
                Parameters = {
                        new NpgsqlParameter { Value = id }
                }
        };


        var reader = await db.Query(cmd);
        string pass ="";
        string job ="";
        string name ="";
        

        while (await reader.ReadAsync()) {
          pass = reader["login"].ToString()!;
          job = reader["job"].ToString()!;
          name = reader["name"].ToString()!;
        }

        // verify password hash
        //  "wait" == "$2a$11$q2wbzzvC52SpWo5h5Zs54ejhNoIz6nQ0Z7EIrI12x5h9a2fbmYGgu"
        //  Assert.True(BCrypt.HashPassword("wait") == $2a$11$q2wbzzvC52SpWo5h5Zs54ejhNoIz6nQ0Z7EIrI12x5h9a2fbmYGgu)
        bool passwordValid = BCrypt.Net.BCrypt.Verify(Text, pass);
        
        Console.WriteLine($"Password valid: {passwordValid}");

        if (!passwordValid) {
            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            "Error: Invalid credentials")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
            return;
        }
        
        
        Console.WriteLine($"Job: {job}");

        // We have an ID, let's check if it's one of the staff
        if (staff.Contains(job)) {
            Console.WriteLine($"Staff access! {MobileUIRequested} {job} {name}");

            if (MobileUIRequested) {
                Console.WriteLine("User requested mobile interface");
                // end me...
                UIController ncontroller = UIController.GetInstance(null);
                ncontroller.ResizeWindow(400, 800);
            }

            HostScreen.mobileUI = MobileUIRequested;

            HostScreen.CurrentUser = name;
            HostScreen.CurrentUserID = id;

            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            $"Welcome back {name}")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();

            switch (job) {
                case "chef":
                    HostScreen.GoNext(new KitchenViewModel(HostScreen));
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
            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            "Error: Invalid access role")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
        }
    }
}