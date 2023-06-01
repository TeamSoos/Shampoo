using ReactiveUI;

namespace RoutedApp.ViewModels; 

public interface IHostScreen : IScreen {
  public void GoNext(RoutablePage page);
}