using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Notification;
using GUI.Logic.Models.Reservation;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;

namespace GUI.ViewModels;

public class ReservationCardItem {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Selected { get; set; }
    public int Table { get; set; }
}

public class ReservationsModel : ViewModelBase {
    public ReservationsModel(IEnumerable<ReservationCardItem> items) {
        Items = new ObservableCollection<ReservationCardItem>(items);
    }

    public ObservableCollection<ReservationCardItem> Items { get; set; }
}

public class ReservationsViewModel : RoutablePage {
    public ReservationsViewModel(IHostScreen screen) {
        HostScreen = screen;
        GoBack = ReactiveCommand.Create(screen.GoBack);
        Refresh = ReactiveCommand.Create(loadReservations);
        DeleteSelected = ReactiveCommand.Create(() => {
            // Can be optimized
            int count = 0;

            foreach (ReservationCardItem reservation in ReservationsList.Items) {
                if (reservation.Selected == "True") {
                    Console.WriteLine($"TO DELETE {reservation.Selected} - {reservation.Title}");
                    Reservation.Delete(reservation.Table);
                    Logger.addRecord(screen.CurrentUserID,$"Deleted reservation for {reservation.Title}. {reservation.Description}");
                    count++;
                }
            }

            HostScreen.notificationManager.CreateMessage()
                .Animates(true)
                .Background("#B4BEFE")
                .Foreground("#1E1E2E")
                .HasMessage(
                    $"Deleted {count} reservations!")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
            // Update the list, no worky :(
            loadReservations();
        });

        ReservationsList = new ReservationsModel(
            new List<ReservationCardItem>());

        // Loop here and add reservations
        loadReservations();
    }

    public override IHostScreen HostScreen { get; }
    public ReservationsModel ReservationsList { get; set; }

    public ReactiveCommand<Unit, Unit> DeleteSelected { get; set; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public ReactiveCommand<Unit, Unit> Refresh { get; }

    void loadReservations() {
        ReservationsList.Items = new ObservableCollection<ReservationCardItem>();
        // load here
        List<Reservation> reservations = new List<Reservation>();

        Task.Run(async () =>
        {
            reservations = await Reservation.getAll();
        }).Wait();
        
        foreach (Reservation reservation in reservations) {
            Console.WriteLine($"{reservation.name}");
            ReservationsList.Items.Add(
                    new ReservationCardItem {
                            Title = reservation.name, Description = $"Reservation at {reservation.time:HH:mm}",
                            Selected = "False", Table = reservation.table
                    }
            );
        }
    }
}