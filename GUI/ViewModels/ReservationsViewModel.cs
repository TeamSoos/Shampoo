using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using GUI.Logic.Models.Reservation;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels; 

public class ReservationCardItem {
  public string Title { get; set; }
  public string Description { get; set; }
  public string Selected { get; set; }
  public int Table { get; set; }
}

public class ReservationsModel : ViewModelBase {
  public ReservationsModel(IEnumerable<ReservationCardItem> items)
  {
    Items = new ObservableCollection<ReservationCardItem>(items);
  }

  public ObservableCollection<ReservationCardItem> Items { get; set; }
}
public class ReservationsViewModel : RoutablePage {

  public override IHostScreen HostScreen { get; }
  public ReservationsModel ReservationsList { get; set; }

  public ReactiveCommand<Unit, Unit> DeleteSelected { get; set; }
  public ReactiveCommand<Unit, Unit> GoBack { get; }
  public ReactiveCommand<Unit, Unit> Refresh { get; }
  public ReservationsViewModel(IHostScreen screen) {
    HostScreen = screen;
    GoBack = ReactiveCommand.Create(screen.GoBack);
    Refresh = ReactiveCommand.Create(loadReservations);
    DeleteSelected = ReactiveCommand.Create(() =>
    {
      // Can be optimized
      foreach (ReservationCardItem reservation in ReservationsList.Items) {
        if (reservation.Selected == "True") {
          Console.WriteLine($"TO DELETE {reservation.Selected} - {reservation.Title}");
          Reservation.Delete(reservation.Table);
        }
      }
    });
    
    ReservationsList = new ReservationsModel(
        new List<ReservationCardItem>() {}
    );
    
    // Loop here and add reservations
    loadReservations();
  }
  private void loadReservations() {
    ReservationsList.Items = new ObservableCollection<ReservationCardItem>();
    // load here
    Task.Run(async () =>
    {
      foreach (Reservation reservation in await Reservation.getAll()) {
        Console.WriteLine($"{reservation.name}");
        ReservationsList.Items.Add(
                new() {
                    Title = reservation.name, Description = $"Reservation at {reservation.time:HH:mm}", Selected = "False", Table = reservation.table
                }
            );
      }
    }).Wait();
  }
}