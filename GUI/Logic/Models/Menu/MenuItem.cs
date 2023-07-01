using System;
using System.Linq;
using System.Reactive;
using Avalonia.Notification;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Logic.Models.Menu;

public class MenuItem {

    public MenuItem(MenuType menuType, IHostScreen screen) {
        Name = menuType.Name;
        Command = ReactiveCommand.Create(() => {
            // Pass in delegates over here
            //screen.GoNext(new OrderItemInfoViewModel(screen, menuType, AddToOrder, AddNote, RemoveAllFromOrder,
             //   CurrentAmount));
        });
        HostScreen = screen;
        Price = menuType.Price;
        MenuItemFull = menuType;
    }

    //int CurrentAmount => HostScreen.CurrentOrder.Count(x => x == this);

    void AddToOrder(int amount) {
        int amount_display = amount;

        // remove all, we then modify it
        //HostScreen.CurrentOrder.RemoveAll(x => x == this);

        while (amount > 0) {
            //HostScreen.CurrentOrder.Add(this);
            amount--;
        }

        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Added {amount_display} x {Name} to order")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
    }

    void RemoveAllFromOrder() {
        //HostScreen.CurrentOrder.RemoveAll(i => i == this);
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Removed all of {Name} from order")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
    }

    void AddNote(string note) {
        Note = note;
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Added a note to the order")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
    }

    /// <summary>
    /// This one is here for SQL queries. I have unpacked some other values for ease of use in the GUI though...
    /// </summary>
    public MenuType MenuItemFull { get; set; }

    public string Name { get; }
    public string Note { get; set; } = "";
    public decimal Price { get; set; }

    public bool HasStock => MenuItemFull.Count > 0;


    // This is a string which gets reflected in the UI
    public string AddToOrderLabel => HasStock ? "Add to order" : "Out of stock";


    IHostScreen HostScreen;

    public ReactiveCommand<Unit, Unit> Command { get; }
}