using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderMenuViewOrderViewModel : RoutablePage {

    public OrderMenuViewOrderViewModel(IHostScreen screen) {
        HostScreen = screen;
        goBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });
    }

    public OrderMenuViewOrderViewModel(IHostScreen screen, List<Menu> items) {
        HostScreen = screen;
        this.items = items;
        isSet = true;
        goBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });
    }

    public override IHostScreen HostScreen { get; }

    public ReactiveCommand<Unit, Unit> goBack { get; }

    public bool isSet { get; set; }

    public List<Menu> items { get; set; } = new List<Menu>();
}