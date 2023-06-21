using System;
using System.Reactive;
using Avalonia.Notification;
using GUI.Views;
using Logic.Models.Item;
using ReactiveUI;

namespace GUI.ViewModels;

public class InventoryAddItemViewModel : RoutablePage {

    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> BackButton { get; set; }
    
    public ReactiveCommand<Unit, Unit> AddButton { get; set; }
    
    public string ItemID { get; set; }
    public string ItemQuantity { get; set; }

    public InventoryAddItemViewModel(IHostScreen screen, string ID) {
        HostScreen = screen;
        ItemID = ID;
        BackButton = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        AddButton = ReactiveCommand.Create(() => {
            HostScreen.GoNext(new NewPageViewModel(screen));

            HostScreen.notificationManager.CreateMessage()
                    .Animates(true)
                    .Background("#B4BEFE")
                    .Foreground("#1E1E2E")
                    .HasMessage(
                            "Item added!")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                    .Queue();
        });
    }

    public InventoryAddItemViewModel(IHostScreen screen) {
        HostScreen = screen;
        BackButton = ReactiveCommand.Create(() =>
        {
            HostScreen.GoBack();
        });
        
        AddButton = ReactiveCommand.Create(() =>
        {
            int.TryParse(ItemID, out int id);
            int.TryParse(ItemQuantity, out int quantity);
            ItemSQL.add_by_id(id, quantity);
            HostScreen.GoNext(new NewPageViewModel(HostScreen));
            
            HostScreen.notificationManager.CreateMessage()
                .Animates(true)
                .Background("#B4BEFE")
                .Foreground("#1E1E2E")
                .HasMessage(
                    "Item added!")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
        });
    }
}