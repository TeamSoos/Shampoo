namespace GUI.ViewModels;

public class InventoryItemsListViewModel : RoutablePage {

    public InventoryItemsListViewModel(IHostScreen screen) {
        HostScreen = screen;
    }

    public override IHostScreen HostScreen { get; }
}