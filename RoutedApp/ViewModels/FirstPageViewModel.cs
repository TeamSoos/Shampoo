using System;
using ReactiveUI;

namespace RoutedApp.ViewModels;

public class FirstPageViewModel : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
	
	public FirstPageViewModel(IScreen screen) => HostScreen = screen;

	public void A() {
		if (this is MainWindowViewModel) {
			
		}
	}
	
	
}