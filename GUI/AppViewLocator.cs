using System;
using GUI.ViewModels;
using GUI.Views;
using ReactiveUI;
using RoutedApp.ViewModels;
using RoutedApp.Views;

namespace RoutedApp;

public class AppViewLocator : ReactiveUI.IViewLocator {
	public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch {
		LoginPageViewModel context => new LoginPageView { DataContext = context },
		OrderingViewModel context => new OrderingView { DataContext = context },
		TablesViewModel context => new TablesView {DataContext = context},
		SecondPageViewModel context => new SecondPage {DataContext = context},
		_ => throw new ArgumentOutOfRangeException($"you forgot to add {nameof(viewModel)} to AppViewLocator"),
	};
}