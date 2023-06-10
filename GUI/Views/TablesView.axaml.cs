using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using RoutedApp.ViewModels;

namespace GUI.Views; 

public partial class TablesView : ReactiveUserControl<TablesViewModel> {
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  public TablesView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }

  public T get_by_name<T>(string name) where T : class, IControl {
    return this.FindControl<T>(name);
  }
}