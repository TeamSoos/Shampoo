using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class SecondPage : ReactiveUserControl<SecondPageViewModel> {
    public SecondPage() {
        InitializeComponent();
    }

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}