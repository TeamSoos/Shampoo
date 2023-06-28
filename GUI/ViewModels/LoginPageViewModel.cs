using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Notification;
using GUI.Logic.Models.Employee;
using GUI.Views;
using ModelLayer;
using ReactiveUI;
using RoutedApp.Logic;
using ServiceLayer.Employee;

namespace GUI.ViewModels;

public class LoginPageViewModel : RoutablePage {

    bool _mobileui;

    public string LoginInput { get; set; }
    public string IDInput { get; set; }

    public ICommand LoginStaff { get; }
    
    public bool MobileUIRequested {
      get => _mobileui;
      set => this.RaiseAndSetIfChanged(ref _mobileui, value);
    }
    
    public override IHostScreen HostScreen { get; }

    public LoginPageViewModel(IHostScreen screen) {
      MobileUIRequested = false; 
      HostScreen = screen;
      LoginStaff = ReactiveCommand.Create(loginStaffTask);
    }


    public void loginStaffTask() {
        EmployeeService employeeService = new EmployeeService();

        int.TryParse(IDInput, out int id_input);

        if (id_input == 0 || LoginInput == "") {
            HostScreen.Notify("Please fill in all fields!", 5);
            return;
        }

        Employee employee = employeeService.GetOne(id_input);
        
        
        // Job can be null when an empty object is returned
        // This is to handle employees that do not exist
        if (employee.Login == null || !employeeService.Authenticate(employee, LoginInput)) {
            HostScreen.Notify("Sorry! Invalid credentials", 5);
            return;
        }

        if (MobileUIRequested) {
            UIController ncontroller = UIController.GetInstance(null);
            ncontroller.ResizeWindow(400, 800);
        }
        
        // Set info for other views to use
        HostScreen.mobileUI = MobileUIRequested;
        HostScreen.CurrentUser = employee;
        
        HostScreen.Notify($"Welcome back {employee.Name}", 3);

        switch (employee.Job) {
            case EmployeeJob.admin:
                HostScreen.GoNext(new NewPageViewModel(HostScreen));
                break;
            case EmployeeJob.bartender:
                HostScreen.GoNext(new KitchenViewModel(HostScreen));
                break;
            case EmployeeJob.chef:
                HostScreen.GoNext(new KitchenViewModel(HostScreen));
                break;
            case EmployeeJob.waiter:
                HostScreen.GoNext(new TablesViewModel(HostScreen));
                break;
        }
    }
}