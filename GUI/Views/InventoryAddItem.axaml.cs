using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class InventoryAddItem : ReactiveUserControl<InventoryAddItemViewModel>
{
    public InventoryAddItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(block:_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}