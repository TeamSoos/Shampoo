using System;
using System.Reactive;
using GUI.Logic.Models.Menu;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderItemInfoViewModel : RoutablePage {

    public override IHostScreen HostScreen { get; }
    public string Name { get; set; }
    public string Note { get; set; } = "";

    public int OrderAmount { get; set; } = 1;
    public int MaxOrderAmount { get; set; }

    ReactiveCommand<Unit, Unit> AddToOrder { get; set; }
    ReactiveCommand<Unit, Unit> RemoveAll { get; set; }
    ReactiveCommand<Unit, Unit> AddNote { get; set; }
    ReactiveCommand<Unit, Unit> GoBack { get; set; }


    public OrderItemInfoViewModel(IHostScreen screen, MenuType item, Action<int> modifyQuantity, Action<string> addNote,
        Action removeAll, int currOrderAmount) {
        
        OrderAmount = currOrderAmount;
        
        HostScreen = screen;
        Name = $"{item.Name} - €{item.Price}";
        MaxOrderAmount = item.Count;

        AddToOrder = ReactiveCommand.Create(() => { modifyQuantity(OrderAmount); });
        RemoveAll = ReactiveCommand.Create(removeAll);
        AddNote = ReactiveCommand.Create(() => {
            addNote(Note);
        });
        GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });
    }
}