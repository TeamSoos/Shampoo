using System;
using System.Reactive;
using GUI.Logic.Models.Menu;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderItemInfoViewModel : RoutablePage {

    public override IHostScreen HostScreen { get; }
    public string Name { get; set; }
    public string Note { get; set; }

    public int OrderAmount { get; set; } = 1;

    ReactiveCommand<Unit, Unit> AddToOrder { get; set; }
    ReactiveCommand<Unit, Unit> AddNote { get; set; }
    ReactiveCommand<Unit, Unit> GoBack { get; set; }


    public OrderItemInfoViewModel(IHostScreen screen, MenuType item, Action<int> add, Action<string> addNote) {
        HostScreen = screen;
        Note = "";
        Name = item.Name;

        AddToOrder = ReactiveCommand.Create(() => { add(OrderAmount); });
        AddNote = ReactiveCommand.Create(() => { addNote(Note); });
        GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });
    }
}