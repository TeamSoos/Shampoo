using System;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Notification;
using ExCSS;
using GUI.ViewModels;
using ReactiveUI;
using GUI.ViewModels;

namespace GUI.Views;

public class NewPageViewModel: RoutablePage
{
    public override IHostScreen HostScreen { get; }

    public ReactiveCommand<Unit, Unit> Logout { get; set; }
    public ReactiveCommand<Unit, Unit> AddItemButton { get; set; }
    public ReactiveCommand<Unit, Unit> ViewInventory { get; set; }

    public NewPageViewModel(IHostScreen screen)
    {
        HostScreen = screen;
        Logout = ReactiveCommand.Create(logoutUser);
        
            // this (int x) => { return x +1; }  is a lambda

        ViewInventory = ReactiveCommand.Create(
            () =>
            {
                HostScreen.GoNext(new InventoryItemsListViewModel(HostScreen));
            }
        );

        AddItemButton = ReactiveCommand.Create(() =>
        {
            HostScreen.GoNext(new InventoryAddItemViewModel(HostScreen));
        });
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
}