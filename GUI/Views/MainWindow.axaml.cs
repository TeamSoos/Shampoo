using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace RoutedApp.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel> {
	public MainWindow() {
		this.WhenActivated(_ => { });
		AvaloniaXamlLoader.Load(this);
	}
	
}