using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace RoutedApp.Views; 

public partial class SecondPage : ReactiveUserControl<SecondPageViewModel> {
  public SecondPage() {
    InitializeComponent();
  }

  private void InitializeComponent() {
		this.WhenActivated(_ => {});
    AvaloniaXamlLoader.Load(this);
  }
}