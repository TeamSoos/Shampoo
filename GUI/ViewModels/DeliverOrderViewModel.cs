using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Notification;
using GUI.Logic.Models.Order;
using GUI.Logic.Models.Table;
using ReactiveUI;
using RoutedApp.Logic.Models.Logging;

namespace GUI.ViewModels; 

public class OrderCardItem {
  public string Title { get; set; }
  public string Description { get; set; }
  public string Selected { get; set; }
  public int id { get; set; }
}

public class OrderModel : ViewModelBase {
  public ObservableCollection<OrderCardItem> Items { get; set; }
  
  public OrderModel(IEnumerable<OrderCardItem> items) {
    Items = new ObservableCollection<OrderCardItem>(items);
  }

}

public class DeliverOrderViewModel : RoutablePage {
  public OrderModel OrdersList { get; set; }
  public override IHostScreen HostScreen { get; }
  public int CurrentTable { get; set; }
  public ReactiveCommand<Unit, Unit> BackButton { get; set; }
  public ReactiveCommand<Unit, Unit> DeliverSelected { get; set; }

  public DeliverOrderViewModel(IHostScreen screen) {
    HostScreen = screen;
    BackButton = ReactiveCommand.Create(screen.GoBack);
    DeliverSelected = ReactiveCommand.Create(deliverSelected);
    CurrentTable = screen.CurrentTable;
    
    OrdersList = new OrderModel(
        new List<OrderCardItem>());
    
    loadOrders();
  }
  private void deliverSelected() {
    int count = 0;

    foreach (OrderCardItem order in OrdersList.Items) {
      if (order.Selected == "True") {
        OrderType.Deliver(int.Parse(order.Title));
        Logger.addRecord(HostScreen.CurrentUserID,
            $"Delivered order {order.Title}. {order.Description}");
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
            $"Delivered {count} orders!")
        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
        .Queue();

    loadOrders();
  }
  
  void loadOrders() {
    OrdersList.Items.Clear();

    List<OrderType> orders = new List<OrderType>();

    Task.Run(async () => { orders = await TableType.getOrdersTyped(CurrentTable); }).Wait();
    
    foreach (OrderType order in orders) {
      Console.WriteLine($"{order.OrderId} {order.Delivered}");
      if (!order.Delivered) {
        OrdersList.Items.Add(
            new OrderCardItem() {
                Title = $"{order.OrderId}",
                Description = $"Ordered at [INSERT TIME KUNAL]\nItems: [KUNALLLL]",
                Selected = "False", id = order.OrderId
            }
        );
      }
    }
  }
}