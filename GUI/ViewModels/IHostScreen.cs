using Avalonia.Notification;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels; 

public interface IHostScreen : IScreen {
  public void GoNext(RoutablePage page);
  public bool mobileUI { get; set; }

  public INotificationMessageManager notificationManager { get; }
  string CurrentUser { get; set; }
}