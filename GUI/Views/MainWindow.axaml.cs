using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RoutedApp.Logic;
using RoutedApp.ViewModels;

namespace GUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel> {
	public MainWindow() {
		UIController controller = new UIController(this);
		controller.setResizerFunc(this.ResizeWindow);
		
		this.WhenActivated(_ => { });
		this.AttachDevTools();
		AvaloniaXamlLoader.Load(this);
	}
	
	public void ResizeWindow(double width, double height)
	{
		// Update the window size
		Width = width;
		Height = height;
	}
}