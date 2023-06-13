using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.Views; 

public partial class SecondPage : ReactiveUserControl<SecondPageViewModel> {
  public SecondPage() {
    InitializeComponent();
  }

  private void InitializeComponent() {
		this.WhenActivated(_ => {});
    AvaloniaXamlLoader.Load(this);
  }
}