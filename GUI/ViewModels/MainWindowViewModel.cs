using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Menu;
using ModelLayer;
using ReactiveUI;
using RoutedApp;
using RoutedApp.Logic;

namespace GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject, IHostScreen {

    readonly NavigationStack stack;

    public MainWindowViewModel() {
        stack = new NavigationStack(new LoginPageViewModel(this));

        CurrentOrder = new List<MenuItem>();

        // Navigate to the first page
        Router.Navigate.Execute(new LoginPageViewModel(this));
        GoNext(new LoginPageViewModel(this));
        // GoNext(new TablesViewModel(this));
        UIController ncontroller = UIController.GetInstance(null);
        
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
        stack.GetTopPage().OnUnload();
        stack.GoTo(page);
        Router.Navigate.Execute(page);
    }

    public void GoBack() {
        GoBackPage.Execute();
        stack.GetTopPage().OnLoad();
    }

    public void Notify(string msg, int timeout) {
        this.notificationManager.CreateMessage()
                .Animates(true)
                .Background("#B4BEFE")
                .Foreground("#1E1E2E")
                .HasMessage(msg)
                .Dismiss().WithDelay(TimeSpan.FromSeconds(timeout))
                .Queue();
    }
    
    public void LogoutUserAction() {
        this.notificationManager.CreateMessage()
                .Animates(true)
                .Background("#B4BEFE")
                .Foreground("#1E1E2E")
                .HasMessage(
                        $"Logging out. Good bye {this.CurrentUser.Name}!")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
        this.GoNext(new LoginPageViewModel(this));
    }

    public bool mobileUI { get; set; }

    public INotificationMessageManager notificationManager => Manager;

    public Employee CurrentUser { get; set; }
    public int CurrentUserID {
        get => CurrentUser.ID;
        set {} 
    }
    public int CurrentTable { get; set; }
    public int GuestCount { get; set; }
}