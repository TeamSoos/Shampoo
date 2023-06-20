using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class SelectTableView : ReactiveUserControl<SelectTableViewModel> {
    public SelectTableView() {
        InitializeComponent();
    }

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }
}