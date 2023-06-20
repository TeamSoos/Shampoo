using System.Collections.Generic;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Menu;
using ReactiveUI;
using RoutedApp;

namespace GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject, IHostScreen {

    readonly NavigationStack stack;

    public MainWindowViewModel() {
        stack = new NavigationStack(new LoginPageViewModel(this));

        CurrentOrder = new List<MenuItem>();

        // Navigate to the first page
        Router.Navigate.Execute(new LoginPageViewModel(this));
        GoNext(new OrderMenuViewModel(this, "test", 1));
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

    public List<MenuItem> CurrentOrder { get; set; }

    public void GoNext(RoutablePage page) {
        stack.GoTo(page);
        Router.Navigate.Execute(page);
    }

    public void GoBack() {
        GoBackPage.Execute();
    }

    public bool mobileUI { get; set; }

    public INotificationMessageManager notificationManager => Manager;

    public string CurrentUser { get; set; }
    public int CurrentTable { get; set; }
    public int GuestCount { get; set; }
}