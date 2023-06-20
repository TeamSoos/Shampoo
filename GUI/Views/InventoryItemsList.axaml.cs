using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class InventoryItemsList : ReactiveUserControl<InventoryItemsListViewModel> {
    public InventoryItemsList() {
        InitializeComponent();
    }

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}