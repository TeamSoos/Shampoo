using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Notification;
using GUI.Logic.Models.Employee;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;

namespace GUI.ViewModels;

public class ManagedUserCardItem {
  public string Title { get; set; }
  public string Description { get; set; }
  public string Selected { get; set; }
  public int id { get; set; }
}

public class ManagedUserModel : ViewModelBase {
  public ManagedUserModel(IEnumerable<ManagedUserCardItem> items) {
    Items = new ObservableCollection<ManagedUserCardItem>(items);
  }

  public ObservableCollection<ManagedUserCardItem> Items { get; set; }
}

public class UserManagementModel : RoutablePage {

  public override IHostScreen HostScreen { get; }

  public UserManagementModel(IHostScreen screen) {
    HostScreen = screen;
    Logout = ReactiveCommand.Create(logoutUser);

    DeleteSelected = ReactiveCommand.Create(deleteUsers);
    ModifySelected = ReactiveCommand.Create(modifyUser);
    AddUser = ReactiveCommand.Create(addUser);
    UpdateUser = ReactiveCommand.Create(updateUser);

    ManagedUserList = new ManagedUserModel(
        new List<ManagedUserCardItem>());

    loadUsers();
  }
  private void updateUser() {
    EmployeeType selectedUser = new EmployeeType(int.Parse(IDField));
    
    selectedUser.Update(LoginField, NameField, (string)JobField.Content);
    
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"User {NameField} with ID {IDField} has been updated!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();

    loadUsers();
  }
  private void modifyUser() {
    ManagedUserCardItem userCardItem = null;
    
    foreach (ManagedUserCardItem user in ManagedUserList.Items) {
      if (user.Selected != "True") {
        continue;
      }
      userCardItem = user;
    }

    EmployeeType employee;

    if (userCardItem != null) {
      employee = new EmployeeType(userCardItem.id);
    }
    else {
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"No user selected to modify!")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
          .Queue();
      return;
    }

    NameField = employee.name;
    JobIndex = employee.job switch {
        "chef" => 0,
        "bartender" => 1,
        "waiter" => 2,
        "admin" => 3,
        _ => throw new ArgumentOutOfRangeException()
    };
    LoginField = "[hashed]";
    IDField = employee.id.ToString();

    loadUsers();
  }
  private void addUser() {
    if (NameField == "") {
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"Name field can not be empty!")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
          .Queue();
      return;
    }
    if ((string)JobField.Content == "") {
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"Job field can not be empty")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
          .Queue();
      return;
    }
    if (LoginField == "") {
      HostScreen.notificationManager.CreateMessage()
          .Animates(true)
          .Background("#B4BEFE")
          .Foreground("#1E1E2E")
          .HasMessage(
              $"Login field can not be empty")
          .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
          .Queue();
      return;
    }

    EmployeeType.Create(NameField, (string)JobField.Content, LoginField);
    
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"New user {NameField} has been created!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();

    loadUsers();
  }

  void deleteUsers() {
    int count = 0;

    foreach (ManagedUserCardItem user in ManagedUserList.Items) {
      if (user.Selected == "True") {
        Console.WriteLine($"TO DELETE {user.Selected} - {user.Title} - {user.id}");
        EmployeeType.Delete(user.id);
        Logger.addRecord(HostScreen.CurrentUserID,
            $"Deleted user {user.Title}. {user.Description}");
        count++;
      }
    }

    if (count == 0)
      return;

    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Deleted {count} users!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();

    loadUsers();
  }

  private void logoutUser() {
    HostScreen.notificationManager.CreateMessage()
        .Animates(true)
        .Background("#B4BEFE")
        .Foreground("#1E1E2E")
        .HasMessage(
            $"Logging out. Good bye {HostScreen.CurrentUser}!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();
    HostScreen.GoNext(new LoginPageViewModel(HostScreen));
  }
  public ReactiveCommand<Unit, Unit> Logout { get; set; }
  public ReactiveCommand<Unit, Unit> DeleteSelected { get; set; }
  public ReactiveCommand<Unit, Unit> AddUser { get; set; }
  public ManagedUserModel ManagedUserList { get; set; }
  public ReactiveCommand<Unit, Unit> BackButton { get; set; }

  private string nameField;
  public string NameField {
    get => nameField;
    set => this.RaiseAndSetIfChanged(ref nameField, value);
  }
  private string idField;
  public string IDField {
    get => idField;
    set => this.RaiseAndSetIfChanged(ref idField, value);
  }
  
  private ComboBoxItem jobField;
  public ComboBoxItem JobField {
    get => jobField;
    set => this.RaiseAndSetIfChanged(ref jobField, value);
  }
  
  private string loginField;
  public string LoginField {
    get => loginField;
    set => this.RaiseAndSetIfChanged(ref loginField, value);
  }
  public ReactiveCommand<Unit, Unit> ModifySelected { get; set; }
  
  private int jobIndex;
  public int JobIndex {
    get => jobIndex;
    set => this.RaiseAndSetIfChanged(ref jobIndex, value);
  }
  public ReactiveCommand<Unit,Unit> UpdateUser { get; set; }

  void loadUsers() {
    ManagedUserList.Items.Clear();

    List<EmployeeType> users = new List<EmployeeType>();
    BackButton = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

    Task.Run(async () => { users = await EmployeeType.getAll(); }).Wait();

    foreach (EmployeeType user in users) {
      ManagedUserList.Items.Add(
          new ManagedUserCardItem {
              Title = user.name, Description = $"ID is {user.id}. Works as {user.job}",
              Selected = "False", id = user.id
          }
      );
    }
  }
}