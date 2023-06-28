namespace ModelLayer.OrderMenu;

public class OrderModel {
    List<OrderMenuItemModel> _order;

    public List<OrderMenuItemModel> Order {
        get => _order;
        set {
        }
    }

    private List<OrderMenuItemModel> ManyToOne(List<OrderMenuItemModel> order) {
        
    }


    // It would probably be fine if we move the UI layer
    // However it would be convinient to have this code here
    public List<Dictionary<string, OrderMenuItemModel>> OrderWithHeading {
        get {
            return Order.GroupBy(item => item.Type)
                .Select(group =>
                    new Dictionary<string, OrderMenuItemModel> {
                        { group.Key, group.First() }
                    })
                .ToList();
        }
    }

    public void AddItem(OrderMenuItemModel item) {
        
    }
}