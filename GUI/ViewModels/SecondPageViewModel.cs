using ReactiveUI;

namespace RoutedApp.ViewModels; 

public class SecondPageViewModel: RoutablePage {

  public override IHostScreen HostScreen { get; }

  public SecondPageViewModel(IHostScreen screen) {
    HostScreen = screen;
    
  }
}