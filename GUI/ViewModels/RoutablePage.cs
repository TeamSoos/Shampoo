using System;
using ReactiveUI;

namespace GUI.ViewModels; 

public abstract class RoutablePage : ReactiveObject, IRoutableViewModel {
	public abstract IScreen HostScreen { get; }
	public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
}