using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace RoutedApp.Views; 

public partial class FirstPage : ReactiveUserControl<FirstPageViewModel> {
	public FirstPage() {
		InitializeComponent();
	}

	private void InitializeComponent() {
		this.WhenActivated(_ => {});
		AvaloniaXamlLoader.Load(this);
	}
}