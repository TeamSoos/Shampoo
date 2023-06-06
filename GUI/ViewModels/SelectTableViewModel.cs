using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using GUI.Logic.Models.Employee;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class SelectTableViewModel : RoutablePage {

  public override IHostScreen HostScreen { get; }
  private ObservableCollection<ComboBoxItem> comboBoxItems;

  public ObservableCollection<ComboBoxItem> ComboBoxItems {
    get { return comboBoxItems; }
    set { this.RaiseAndSetIfChanged(ref comboBoxItems, value); }
  }

  public ReactiveCommand<Unit, Unit> CreateOrder { get; }
  public ReactiveCommand<Unit, Unit> GoBack { get; }

#pragma warning disable CS8618
  public SelectTableViewModel(IHostScreen screen) {
#pragma warning restore CS8618
    HostScreen = screen;

    CreateOrder = ReactiveCommand.Create(() => {
      HostScreen.GoNext(new OrderMenuViewModel(HostScreen));
    });
    
    GoBack = ReactiveCommand.Create(() => {
      HostScreen.GoBack();
    });


    // Initialize the ComboBoxItems collection
    ComboBoxItems = new ObservableCollection<ComboBoxItem>();
    loadWaiters();
  }

  void loadWaiters() {
    // Collect all waiters
    List<EmployeeType> employees = new List<EmployeeType>();
    Task.Run(async () => {
      employees = await EmployeeType.getAll("waiter");

      Console.WriteLine("Done getting waiters");
    }).Wait();

    foreach (ComboBoxItem item in employees.Select(employee => new ComboBoxItem { Content = employee.name })) {
      ComboBoxItems.Add(item);
    }
  }
}