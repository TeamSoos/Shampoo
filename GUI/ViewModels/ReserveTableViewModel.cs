using System;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Reservation;
using ReactiveUI;

namespace GUI.ViewModels;

public class ReserveTableViewModel : RoutablePage {

    public ReserveTableViewModel(IHostScreen screen) {
        HostScreen = screen;
        CurrentTable = screen.CurrentTable;
        Time = "00:00";

        /*
         * Guest count can be aqcuired via: screen.GuestCount
         */

        CreateReservation = ReactiveCommand.Create(createReservation);
        GoBack = ReactiveCommand.Create(screen.GoBack);
    }

    public override IHostScreen HostScreen { get; }
    public int CurrentTable { get; set; }
    public ReactiveCommand<Unit, Unit> CreateReservation { get; set; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public string Name { get; set; }

    public string Phone { get; set; }

    // Time will need to be of some specific type. Idk what yet, for now placeholder
    public string Time { get; set; }

    void createReservation() {
        // This shit goes to db
        Console.WriteLine($"{Name} {Phone} {Time}");
        // This will call the db trigger to set the table to occupied
        Reservation.Create(Name, Phone, Time, HostScreen.GuestCount, CurrentTable);
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Reservation for {Name} at {Time} was created!")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();

        HostScreen.GoNext(new TablesViewModel(HostScreen));
    }
}