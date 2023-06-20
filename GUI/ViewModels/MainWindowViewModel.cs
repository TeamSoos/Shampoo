using System.Reactive;
using Avalonia.Notification;
using ReactiveUI;
using RoutedApp;

namespace GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject, IHostScreen {

    readonly NavigationStack stack;

    public MainWindowViewModel() {
        stack = new NavigationStack(new LoginPageViewModel(this));


        // Navigate to the first page
        Router.Navigate.Execute(new LoginPageViewModel(this));
        // Router.Navigate.Execute(new OrderMenuViewModel(this));
        GoBackPage = ReactiveCommand.CreateFromObservable(
            () => {
                RoutablePage _ = stack.Pop();
                return Router.Navigate.Execute(stack.GetTopPage());
            }
        );
    }

    public string Greeting =>
        "Uh oh, something went wrong! Do not fret, for a restart of the app shall fix this predicament!!";

    public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

    public ReactiveCommand<Unit, IRoutableViewModel> GoBackPage { get; }

    public RoutingState Router { get; } = new RoutingState();

    public void GoNext(RoutablePage page) {
        stack.GetTopPage().OnUnload();
        stack.GoTo(page);
        Router.Navigate.Execute(page);
    }

    public void GoBack() {
        GoBackPage.Execute();
        stack.GetTopPage().OnLoad();
    }

    public bool mobileUI { get; set; }

    public INotificationMessageManager notificationManager => Manager;

    public string CurrentUser { get; set; }
    public int CurrentTable { get; set; }
    public int GuestCount { get; set; }
}