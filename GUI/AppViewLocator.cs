using System;
using GUI.ViewModels;
using GUI.Views;
using ReactiveUI;
using RoutedApp.ViewModels;
using RoutedApp.Views;
using SecondPage = GUI.Views.SecondPage;

namespace RoutedApp;

public class AppViewLocator : ReactiveUI.IViewLocator {
	public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch {
		LoginPageViewModel context => new LoginPageView { DataContext = context },
		OrderingViewModel context => new OrderingView { DataContext = context },
		TablesViewModel context => new TablesView {DataContext = context},
		SelectTableViewModel context => new SelectTableView {DataContext = context},
		SecondPageViewModel context => new SecondPage {DataContext = context},
		NewPageViewModel context => new NewPage {DataContext = context},
		OrderMenuViewModel context => new OrderMenu {DataContext = context},
		InventoryItemsListViewModel context => new InventoryItemsList {DataContext = context},
		InventoryAddItemViewModel context => new InventoryAddItem {DataContext = context},
		
		OrderMenuViewOrderViewModel context => new OrderMenuViewOrder {DataContext = context},
		ReserveTableViewModel context => new ReserveTable {DataContext = context},
		ReservationsViewModel context => new ReservationsView {DataContext = context},
		_ => throw new ArgumentOutOfRangeException($"you forgot to add {nameof(viewModel)} to AppViewLocator"),
	};
}