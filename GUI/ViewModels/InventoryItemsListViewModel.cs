using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class InventoryItemsListViewModel: RoutablePage
{
    public override IHostScreen HostScreen { get; }

    public InventoryItemsListViewModel(IHostScreen screen)
    {
        HostScreen = screen;
    }
}