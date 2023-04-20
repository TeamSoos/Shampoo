using System.Windows.Input;
using ReactiveUI;

namespace GUI.ViewModels;

public class MainWindowViewModel : ViewModelBase {

  public MainWindowViewModel() {
    _CurrentPage = new LoginPageViewModel();
    NavigateToApp = ReactiveCommand.Create(GotoApp);
  }
  
  private RouterPage _CurrentPage;

  RouterPage CurrentPage {
    get => _CurrentPage;
    set => this.RaiseAndSetIfChanged(ref _CurrentPage, value);
  }

  
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;

  public ICommand NavigateToApp { get; }

  bool _visible = true;
  bool IsVisible {
    get => _visible;
    set => this.RaiseAndSetIfChanged(ref _visible, value);
  }

  public void GotoApp() {
    
    // Auth logic goes here
    CurrentPage = new BasicAppViewModel();
    IsVisible = false;
  }
}