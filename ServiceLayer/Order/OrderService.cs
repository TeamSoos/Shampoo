using DataLayer.Order;
using ModelLayer.OrderMenu;
using ModelLayer.OrderModel;
using ModelLayer.Tables;

namespace ServiceLayer.Order;

public class OrderService {
    private OrderSQL _sql;

    public OrderService() {
        _sql = new OrderSQL();
    }

    public ModelLayer.Order GetOne(int orderID) {
        return _sql.get_one(orderID);
    }

    public List<ModelLayer.Order> GetAllByTable(Table table) {
        return _sql.get_all_by_table(table);
    }

    public void CreateNewOrder(Table table, List<OrderMenuItemModel> orderItems) {
        var order = _sql.create_order(table);
        Console.WriteLine("HI");

        // we also need to create a new order_item for each menu_item
        var list = orderItems.Select(item => new OrderedItem {
                OrderID = order.ID, ItemID = item.ID, Quantity = item.OrderedCount, Note = item.Note,
            })
            .ToList();
        
        // we need to create a new service here
        var orderedItemService = new OrderedItemService();
        orderedItemService.AddOrderedItems(list);
    }
}