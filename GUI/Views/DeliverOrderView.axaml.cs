using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;

namespace GUI.Views; 

public partial class DeliverOrderView : ReactiveUserControl<DeliverOrderViewModel> {
  public DeliverOrderView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }
}