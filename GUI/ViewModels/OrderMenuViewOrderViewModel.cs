using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Menu;
using GUI.Logic.Models.Order;
using ModelLayer.OrderMenu;
using ReactiveUI;
using ServiceLayer.Order;

namespace GUI.ViewModels;

public class OrderMenuViewOrderViewModel : RoutablePage {
    OrderService service = new();

    public OrderMenuViewOrderViewModel(IHostScreen screen) {
        HostScreen = screen;
        var table = screen.CurrentTable;
        isSet = true;
        goBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });


        Items = screen.CurrentOrder.OrderItems;

        PlaceOrder = ReactiveCommand.Create(() => {
            //OrderSql.place_order(ItemsGrouped, table);
            service.CreateNewOrder(HostScreen.CurrentTable, Items);
            
            // reset the items
            HostScreen.CurrentOrder.OrderItems.Clear();

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

    public List<OrderMenuItemModel> Items { get; set; }
}