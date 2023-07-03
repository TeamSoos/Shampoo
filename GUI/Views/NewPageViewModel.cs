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
    public ReactiveCommand<Unit, Unit> ViewUsers { get; set; }
    public ReactiveCommand<Unit, Unit> ViewRevenueReport { get; set; }

    public NewPageViewModel(IHostScreen screen)
    {
        HostScreen = screen;
        Logout = ReactiveCommand.Create(HostScreen.LogoutUserAction);
        

        ViewInventory = ReactiveCommand.Create(
            () =>
            {
                HostScreen.GoNext(new InventoryItemsListViewModel(HostScreen));
            }
        );
        
        ViewUsers = ReactiveCommand.Create(
                () =>
                {
                    HostScreen.GoNext(new UserManagementModel(HostScreen));
                }
        );
        
        ViewRevenueReport = ReactiveCommand.Create(
            () =>
            {
                HostScreen.GoNext(new RevenueReportViewModel(HostScreen));
            }
        );

        AddItemButton = ReactiveCommand.Create(() =>
        {
            HostScreen.GoNext(new InventoryAddItemViewModel(HostScreen));
        });
    }
    
}