using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;

namespace GUI.Views;

public partial class TablesView : ReactiveUserControl<TablesViewModel> {
    // END STYLING
    public TablesView() {
        InitializeComponent();
    }

    // Styling
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;

    void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }

    public T get_by_name<T>(string name) where T : class, IControl {
        return this.FindControl<T>(name);
    }
}