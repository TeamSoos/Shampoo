using System;
using ReactiveUI;

namespace RoutedApp.ViewModels; 

public abstract class RoutablePage : ReactiveObject, IRoutableViewModel {
	public IScreen HostScreen { get; }
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];

	RoutablePage(IScreen screen) => HostScreen = screen;
}