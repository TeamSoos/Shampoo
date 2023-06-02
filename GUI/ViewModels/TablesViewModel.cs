using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Media;
using Avalonia.Notification;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class TablesViewModel : RoutablePage {
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING

  public TablesViewModel(IHostScreen screen) {
    LogoutUser = ReactiveCommand.Create(logoutUser);
    HostScreen = screen;
  }
  public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }

  public void logoutUser() {HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Logging out. Good bye {HostScreen.CurrentUser}")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();
    HostScreen.GoNext(new LoginPageViewModel(HostScreen));
  }

  public override IHostScreen HostScreen { get; }
}