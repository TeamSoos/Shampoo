using DataLayer.Order;
using ModelLayer.OrderModel;

namespace ServiceLayer.Order; 

public class OrderedItemService {
    private OrderedItemSql _sql = new();
    
    public void AddOrderedItems(List<OrderedItem> items) {
        items.ForEach((item) => {
            _sql.add_ordered_item(item);
        });
    }
    
    public void AddOrderedItem(OrderedItem item) {
        _sql.add_ordered_item(item);
    }
}