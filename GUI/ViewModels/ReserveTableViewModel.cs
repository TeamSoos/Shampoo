using System;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Reservation;
using ModelLayer.Tables;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;
using ServiceLayer.Reservation;

namespace GUI.ViewModels;

public class ReserveTableViewModel : RoutablePage {
    private ReservationService _service;

    public ReserveTableViewModel(IHostScreen screen) {
        HostScreen = screen;
        CurrentTable = screen.CurrentTable;
        Time = "00:00";
                
        _service = new ReservationService();
        
        /*
         * Guest count can be aqcuired via: screen.GuestCount
         */

        CreateReservation = ReactiveCommand.Create(createReservation);
        GoBack = ReactiveCommand.Create(screen.GoBack);
    }

    public override IHostScreen HostScreen { get; }
    public Table CurrentTable { get; set; }
    public ReactiveCommand<Unit, Unit> CreateReservation { get; set; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public string Name { get; set; }

    public string Phone { get; set; }

    // Time will need to be of some specific type. Idk what yet, for now placeholder
    public string Time { get; set; }

    void createReservation() {
        // This will call the db trigger to set the table to occupied
        // Reservation.Create(Name, Phone, Time, HostScreen.GuestCount, CurrentTable);

        _service.Reserve(
                new ModelLayer.Tables.Reservation {
                        Name = Name,
                        Guests = HostScreen.GuestCount,
                        Phone = Phone,
                        Table = CurrentTable
                }
        );
        
        HostScreen.Notify($"Reservation for {Name} at {Time} was created!", 5);
        
        Logger.addRecord(HostScreen.CurrentUserID,$"Created reservation for {Name} at {Time}");
        
        HostScreen.GoNext(new TablesViewModel(HostScreen));
    }
}