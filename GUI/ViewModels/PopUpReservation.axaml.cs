using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.ViewModels;

public partial class PopUpReservation : UserControl {
    public PopUpReservation() {
        InitializeComponent();
    }

    void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}