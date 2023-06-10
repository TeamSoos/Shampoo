using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views; 

public partial class OrderMenu : ReactiveUserControl<OrderMenuViewModel> {
  public OrderMenu() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    this.WhenActivated(_ => { });
    AvaloniaXamlLoader.Load(this);
  }
}