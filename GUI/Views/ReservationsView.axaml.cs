using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;

namespace GUI.Views;

public partial class ReservationsView : ReactiveUserControl<ReservationsViewModel> {
    public ReservationsView() {
        InitializeComponent();
    }

    void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}