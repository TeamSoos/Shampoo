using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.Views; 

public partial class SelectTableView : ReactiveUserControl<SelectTableViewModel> {
  public SelectTableView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    this.WhenActivated(_ => {});
    AvaloniaXamlLoader.Load(this);
  }
}