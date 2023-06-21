namespace GUI.ViewModels;

public class FirstPageViewModel : RoutablePage {

    public FirstPageViewModel(IHostScreen screen) {
        HostScreen = screen;
    }

    public override IHostScreen HostScreen { get; }

    public void A(RoutablePage page) {
        HostScreen.GoNext(new SecondPageViewModel(HostScreen));
    }
}