using Avalonia.Notification;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels; 

public interface IHostScreen : IScreen {
  public void GoNext(RoutablePage page);
  public void GoBack();
  public bool mobileUI { get; set; }

  public INotificationMessageManager notificationManager { get; }
  string CurrentUser { get; set; }
  int CurrentTable { get; set; }
}