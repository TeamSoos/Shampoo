using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel> {
	public MainWindow() {
		this.WhenActivated(_ => { });
		this.AttachDevTools();
		AvaloniaXamlLoader.Load(this);
	}
}