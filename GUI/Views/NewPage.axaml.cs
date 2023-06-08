using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace GUI.Views;

public partial class NewPage : ReactiveUserControl<NewPageViewModel> 
{
    public NewPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(block: _ => {});
        AvaloniaXamlLoader.Load(this);
    }
}