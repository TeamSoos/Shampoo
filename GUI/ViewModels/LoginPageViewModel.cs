using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Notification;
using GUI.Logic.Models.Employee;
using GUI.Views;
using ReactiveUI;
using RoutedApp.Logic;

namespace GUI.ViewModels;

public class LoginPageViewModel : RoutablePage {

    bool _mobileui;

    readonly List<string> staff = new List<string> { "admin", "chef", "barman", "waiter" };

#pragma warning disable CS8618
    public LoginPageViewModel(IHostScreen screen) {
#pragma warning restore CS8618
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
                            "Sorry! Invalid ID")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
            return;
        }
        if (Text == "") {
          HostScreen.notificationManager.CreateMessage()
              .Animates(true)
              .Background("#B4BEFE")
              .Foreground("#1E1E2E")
              .HasMessage(
                  "Sorry! Please fill in the login code")
              .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
              .Queue();
          return;
        }

        EmployeeType user = await EmployeeType.Authenticate(Text, id);

        

        // verify password hash
        //  "wait" == "$2a$11$q2wbzzvC52SpWo5h5Zs54ejhNoIz6nQ0Z7EIrI12x5h9a2fbmYGgu"
        //  Assert.True(BCrypt.HashPassword("wait") == $2a$11$q2wbzzvC52SpWo5h5Zs54ejhNoIz6nQ0Z7EIrI12x5h9a2fbmYGgu)
        bool passwordValid = BCrypt.Net.BCrypt.Verify(Text, user.login);

        if (!passwordValid) {
            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            "Invalid credentials for this user")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
            return;
        }

        // We have an ID, let's check if it's one of the staff
        if (staff.Contains(user.job)) {
            Console.WriteLine($"Staff access! {MobileUIRequested} {user.job} {user.name}");

            if (MobileUIRequested) {
                Console.WriteLine("User requested mobile interface");
                // :)
                UIController ncontroller = UIController.GetInstance(null);
                ncontroller.ResizeWindow(400, 800);
            }

            // Set info for other views to use
            HostScreen.mobileUI = MobileUIRequested;
            HostScreen.CurrentUser = user.name;
            HostScreen.CurrentUserID = user.id;

            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            $"Welcome back {user.name}")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();

            switch (user.job) {
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
                default:
                    HostScreen.notificationManager.CreateMessage()
                            .Animates(true)
                            .Background("#B4BEFE")
                            .Foreground("#1E1E2E")
                            .HasMessage(
                                    "Error: Job not implemented")
                            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                            .Queue();
                    break;
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