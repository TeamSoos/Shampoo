using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Notification;
using GUI.Logic.Models.Employee;
using ModelLayer;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;
using ServiceLayer.Employee;

namespace GUI.ViewModels;

public class ManagedUserCardItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Selected { get; set; }
    public int id { get; set; }
}

public class ManagedUserModel : ViewModelBase
{
    public ManagedUserModel(IEnumerable<ManagedUserCardItem> items)
    {
        Items = new ObservableCollection<ManagedUserCardItem>(items);
    }

    public ObservableCollection<ManagedUserCardItem> Items { get; set; }
}

public class UserManagementModel : RoutablePage
{
    public override IHostScreen HostScreen { get; }
    public EmployeeService service;

    public UserManagementModel(IHostScreen screen)
    {
        HostScreen = screen;
        Logout = ReactiveCommand.Create(logoutUser);

        DeleteSelected = ReactiveCommand.Create(deleteUsers);
        ModifySelected = ReactiveCommand.Create(modifyUser);
        AddUser = ReactiveCommand.Create(addUser);
        UpdateUser = ReactiveCommand.Create(updateUser);

        service = new EmployeeService();

        ManagedUserList = new ManagedUserModel(
            new List<ManagedUserCardItem>());

        loadUsers();
    }

    private void updateUser()
    {
        EmployeeType selectedUser = new EmployeeType(int.Parse(IDField));

        selectedUser.Update(LoginField, NameField, (string)JobField.Content);
        

        HostScreen.Notify($"User {NameField} with ID {IDField} has been updated!", 5);

        loadUsers();
    }

    private void modifyUser()
    {
        ManagedUserCardItem userCardItem = null;

        foreach (ManagedUserCardItem user in ManagedUserList.Items)
        {
            if (user.Selected != "True")
            {
                continue;
            }

            userCardItem = user;
        }

        Employee employee;

        if (userCardItem != null)
        {
            employee = service.GetOne(userCardItem.id);
        }
        else
        {
            HostScreen.Notify($"No user selected to modify!", 5);
            return;
        }

        NameField = employee.Name;
        JobIndex = employee.Job.ToString() switch
        {
            "chef" => 0,
            "bartender" => 1,
            "waiter" => 2,
            "admin" => 3,
            _ => throw new ArgumentOutOfRangeException()
        };
        LoginField = "[hashed]";
        IDField = employee.ID.ToString();

        loadUsers();
    }

    private void addUser()
    {
        // clear IDField
        IDField = "";
        if (NameField == "")
        {
            HostScreen.Notify( $"Name field can not be empty!", 5);
            return;
        }

        if ((string)JobField.Content == "")
        {
            HostScreen.Notify( $"Job field can not be empty", 5);
            return;
        }

        if (LoginField == "")
        {
            HostScreen.Notify( $"Login field can not be empty",5);
            return;
        }

        EmployeeType.Create(NameField, (string)JobField.Content, LoginField);

        HostScreen.Notify( $"New user {NameField} has been created!", 5);

        loadUsers();
    }

    void deleteUsers()
    {
        int count = 0;

        foreach (ManagedUserCardItem user in ManagedUserList.Items)
        {
            if (user.Selected == "True")
            {
                EmployeeType.Delete(user.id);
                Logger.addRecord(HostScreen.CurrentUserID,
                    $"Deleted user {user.Title}. {user.Description}");
                count++;
            }
        }

        if (count == 0)
            return;

        HostScreen.Notify($"Deleted {count} users!", 5);
        loadUsers();
    }

    private void logoutUser()
    {
        HostScreen.Notify($"Logging out. Good bye {HostScreen.CurrentUser}!", 5);
        HostScreen.GoNext(new LoginPageViewModel(HostScreen));
    }

    public ReactiveCommand<Unit, Unit> Logout { get; set; }
    public ReactiveCommand<Unit, Unit> DeleteSelected { get; set; }
    public ReactiveCommand<Unit, Unit> AddUser { get; set; }
    public ManagedUserModel ManagedUserList { get; set; }
    public ReactiveCommand<Unit, Unit> BackButton { get; set; }

    private string nameField;

    public string NameField
    {
        get => nameField;
        set => this.RaiseAndSetIfChanged(ref nameField, value);
    }

    private string idField;

    public string IDField
    {
        get => idField;
        set => this.RaiseAndSetIfChanged(ref idField, value);
    }

    private ComboBoxItem jobField;

    public ComboBoxItem JobField
    {
        get => jobField;
        set => this.RaiseAndSetIfChanged(ref jobField, value);
    }

    private string loginField;

    public string LoginField
    {
        get => loginField;
        set => this.RaiseAndSetIfChanged(ref loginField, value);
    }

    public ReactiveCommand<Unit, Unit> ModifySelected { get; set; }

    private int jobIndex;

    public int JobIndex
    {
        get => jobIndex;
        set => this.RaiseAndSetIfChanged(ref jobIndex, value);
    }

    public ReactiveCommand<Unit, Unit> UpdateUser { get; set; }

    void loadUsers()
    {
        ManagedUserList.Items.Clear();

        List<Employee> users = new List<Employee>();
        BackButton = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        users = service.GetAll();

        foreach (Employee user in users)
        {
            ManagedUserList.Items.Add(
                new ManagedUserCardItem
                {
                    Title = user.Name, Description = $"ID is {user.ID}. Works as {user.Job.ToString()}",
                    Selected = "False", id = user.ID
                }
            );
        }
    }
}