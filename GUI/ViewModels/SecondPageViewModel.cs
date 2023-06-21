namespace GUI.ViewModels;

public class SecondPageViewModel : RoutablePage {

    public SecondPageViewModel(IHostScreen screen) {
        HostScreen = screen;

    }

    public override IHostScreen HostScreen { get; }
}