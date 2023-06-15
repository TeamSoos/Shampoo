using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderMenuViewOrderViewModel : RoutablePage {

    public OrderMenuViewOrderViewModel(IHostScreen screen) {
        HostScreen = screen;
        this.Items = screen.CurrentOrder;
        isSet = true;
        goBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        PlaceOrder = ReactiveCommand.Create(() => { });
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
                    Name = group.Key,
                    Quantity = group.Count(),
                    Price = group.First().Price
                })
                .ToList();
        }
    }
}

public class MenuItemGrouped {
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}