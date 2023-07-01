using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Menu;
using GUI.Logic.Models.Order;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderMenuViewOrderViewModel : RoutablePage {

    public OrderMenuViewOrderViewModel(IHostScreen screen) {
        HostScreen = screen;
        this.Items = screen.CurrentOrder;
        var table = screen.CurrentTable;
        isSet = true;
        goBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        PlaceOrder = ReactiveCommand.Create(() => {
            OrderSql.place_order(ItemsGrouped, table.ID);

            // reset the order
            HostScreen.CurrentOrder = new List<MenuItem>();

            HostScreen.notificationManager.CreateMessage()
                .Animates(true)
                .Background("#B4BEFE")
                .Foreground("#1E1E2E")
                .HasMessage(
                    $"Order placed for table {table}")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
            
            HostScreen.GoBack();
        });
    }

    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> goBack { get; }
    public ReactiveCommand<Unit, Unit> PlaceOrder { get; }
    public bool isSet { get; set; }

    public List<MenuItem> Items { get; set; }

    public List<MenuItemGrouped> ItemsGrouped {
        get {
            return Items
                .GroupBy(item => item.Name)
                .Select(group => new MenuItemGrouped {
                    item = group.First().MenuItemFull,
                    Name = group.Key,
                    Quantity = group.Count(),
                    Price = group.First().Price,
                    Note = group.First().Note
                })
                .ToList();
        }
    }
}

public class MenuItemGrouped {
    public MenuType item;

    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public string Note { get; set; } = "";
}