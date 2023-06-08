﻿using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Notification;
using GUI.ViewModels;
using GUI.Views;
using ReactiveUI;

namespace RoutedApp.ViewModels;

public class MainWindowViewModel : ReactiveObject, IHostScreen {
  public string Greeting =>
    "Uh oh, something went wrong! Do not fret, for a restart of the app shall fix this predicament!!";

  public RoutingState Router { get; } = new RoutingState();

  public void GoNext(RoutablePage page) {
    stack.GoTo(page);
    Router.Navigate.Execute(page);
  }

  public void GoBack() {
    GoBackPage.Execute().Subscribe();
  }

  public bool mobileUI { get; set; }
  public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

  public INotificationMessageManager notificationManager {
    get { return this.Manager; }
  }

  public string CurrentUser { get; set; }
  public int CurrentTable { get; set; }

  readonly NavigationStack stack;

  public ReactiveCommand<Unit, IRoutableViewModel> GoBackPage { get; }

  public MainWindowViewModel() {
    stack = new NavigationStack(new NewPageViewModel(this));


    // Navigate to the first page
    Router.Navigate.Execute(new LoginPageViewModel(this));
    GoBackPage = ReactiveCommand.CreateFromObservable(
      () => {
        var _ = stack.Pop();
        return Router.Navigate.Execute(stack.GetTopPage());
      }
    );
  }
}