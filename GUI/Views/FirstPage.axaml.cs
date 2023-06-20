using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace RoutedApp.Views;

public partial class FirstPage : ReactiveUserControl<FirstPageViewModel> {
    public FirstPage() {
        InitializeComponent();
    }

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}