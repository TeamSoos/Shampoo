namespace GUI.ViewModels;

public class FirstPageViewModel : RoutablePage {
  public override IHostScreen HostScreen { get; }

  public FirstPageViewModel(IHostScreen screen) {
    HostScreen = screen;
  }

  public void A(RoutablePage page) {
    HostScreen.GoNext(new SecondPageViewModel(HostScreen));
  }

}