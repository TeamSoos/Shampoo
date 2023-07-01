using System.Collections.Generic;
using Avalonia.Notification;
using GUI.Logic.Models.Menu;
using ModelLayer;
using ModelLayer.Tables;
using ReactiveUI;

namespace GUI.ViewModels;

public interface IHostScreen : IScreen {
    public bool mobileUI { get; set; }

    public INotificationMessageManager notificationManager { get; }
    
    // state
    Employee CurrentUser { get; set; }
    
    int CurrentUserID { get; set; }
    Table CurrentTable { get; set; }
    int GuestCount { get; set; }
    List<MenuItem> CurrentOrder { get; set; }
    public void LogoutUserAction();
    public void GoNext(RoutablePage page);
    public void GoBack();
    void Notify(string msg, int timeout);
}