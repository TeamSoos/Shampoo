using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class InventoryItemsList : ReactiveUserControl<InventoryItemsListViewModel>
{
    public InventoryItemsList()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(block:_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}