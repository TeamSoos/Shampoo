using System;
using System.Reactive;
using GUI.Logic.Models.Menu;
using ModelLayer.OrderMenu;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderItemInfoViewModel : RoutablePage {

    public override IHostScreen HostScreen { get; }
    public string Name { get; set; }
    public string Note { get; set; } = "";

    public int OrderAmount { get; set; }
    public int MaxOrderAmount { get; set; }

    ReactiveCommand<Unit, Unit> AddToOrder { get; set; }
    ReactiveCommand<Unit, Unit> RemoveAll { get; set; }
    ReactiveCommand<Unit, Unit> AddNote { get; set; }
    ReactiveCommand<Unit, Unit> GoBack { get; set; }

    public OrderItemInfoViewModel(IHostScreen screen, OrderMenuItemModel item) {
        HostScreen = screen;

        OrderAmount = 1;
        Name = item.Name;

        MaxOrderAmount = item.Count;

        AddToOrder = ReactiveCommand.Create(() => { item.OrderedCount += OrderAmount; });

        RemoveAll = ReactiveCommand.Create(() => { item.OrderedCount = 0; });

        AddNote = ReactiveCommand.Create(() => { item.Note = Note; });
        GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });
    }
}