using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.Views; 

public partial class OrderingView : ReactiveUserControl<OrderingViewModel> {
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  public OrderingView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
		this.WhenActivated(_ => { });
    AvaloniaXamlLoader.Load(this);
  }
}