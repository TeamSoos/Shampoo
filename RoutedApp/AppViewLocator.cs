using System;
using ReactiveUI;
using RoutedApp.ViewModels;
using RoutedApp.Views;

namespace RoutedApp;

public class AppViewLocator : ReactiveUI.IViewLocator {
	public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch {
		FirstPageViewModel context => new FirstPage { DataContext = context },
		_ => throw new ArgumentOutOfRangeException(nameof(viewModel)),
	};
}