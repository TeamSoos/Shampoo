using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class OrderingView : ReactiveUserControl<OrderingViewModel> {
    // END STYLING
    public OrderingView() {
        InitializeComponent();
    }

    // Styling
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}