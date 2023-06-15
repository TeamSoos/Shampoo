using System.Collections.Generic;
using Avalonia.Notification;
using ReactiveUI;

namespace GUI.ViewModels;

public interface IHostScreen : IScreen {
    public bool mobileUI { get; set; }

    public INotificationMessageManager notificationManager { get; }
    
    // state
    string CurrentUser { get; set; }
    int CurrentTable { get; set; }
    int GuestCount { get; set; }
    List<MenuItem> CurrentOrder { get; set; }

    public void GoNext(RoutablePage page);
    public void GoBack();
}