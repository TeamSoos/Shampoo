﻿using System.Collections.Generic;
using System.Reactive;
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

  readonly NavigationStack stack;

  public ReactiveCommand<Unit, IRoutableViewModel> GoBackPage { get; }
  
  public MainWindowViewModel() {
    stack = new NavigationStack(new FirstPageViewModel(this));

    // Navigate to the first page
    Router.Navigate.Execute(new FirstPageViewModel(this));
    GoBackPage = ReactiveCommand.CreateFromObservable(
      () => {
        var _ = stack.Pop();
        return Router.Navigate.Execute(stack.GetTopPage());
      }
    );
  }
}