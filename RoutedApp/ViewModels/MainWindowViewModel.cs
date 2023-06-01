using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace RoutedApp.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen {
	public string Greeting => "Welcome to Avalonia!";
	public RoutingState Router { get; } = new RoutingState();

	public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }

	List<IRoutableViewModel> nav_stack = new List<IRoutableViewModel>();

	IRoutableViewModel DefaultPage;
	
	public void PushPage(IRoutableViewModel page) {
		nav_stack.Add(page);
	}
	
	public IRoutableViewModel PopPage() {
		if (nav_stack.Count <= 1) return DefaultPage;
		IRoutableViewModel page = nav_stack[^1];
		nav_stack.RemoveAt(nav_stack.Count - 1);
		return page;
	}
	
	public MainWindowViewModel() {
		DefaultPage = new FirstPageViewModel(this);
		nav_stack.Add(new FirstPageViewModel(this));
		// Navigate to the first page
		Router.Navigate.Execute(new FirstPageViewModel(this));
		GoBack = ReactiveCommand.CreateFromObservable(
			() => Router.Navigate.Execute(PopPage())
		);
	}
}