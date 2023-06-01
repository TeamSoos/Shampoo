using System.Windows.Input;
using ReactiveUI;

namespace GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen {
	public RoutingState Router { get; } = new RoutingState();
}