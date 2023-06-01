using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RoutedApp.ViewModels;
using RoutedApp.Views;

namespace RoutedApp;

public partial class App : Application {
	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new MainWindow {
				DataContext = new MainWindowViewModel(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}