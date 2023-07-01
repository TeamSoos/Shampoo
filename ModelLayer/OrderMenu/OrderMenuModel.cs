namespace ModelLayer.OrderMenu;

public class OrderMenuModel {
    public int ID;
    public bool Paid { get; set; } // gets directly from the database
    public bool Delivered { get; set; } // gets directly from the database
    

    public List<OrderMenuItemModel> OrderItems = new();

    // It would probably be fine if we move the UI layer
    // However it would be convinient to have this code here
    public List<Dictionary<string, OrderMenuItemModel>> OrderWithHeading {
        get {
            return OrderItems.GroupBy(item => item.Type)
                .Select(group =>
                    new Dictionary<string, OrderMenuItemModel> {
                        { group.Key, group.First() }
                    })
                .ToList();
        }
    }

    public void AddItem(OrderMenuItemModel item) {
        // this is by reference
        // I really wish C# was not (TODO: insert an obscenely bad word here)
        // But it is
        // I say the type for this should show up as &T or *T or smthing idk
        var existingItem = OrderItems.FirstOrDefault(x => x.ID == item.ID);
        if (existingItem != null) {
            existingItem.Count += item.Count;
        }
        else {
            OrderItems.Add(item);
        }
    }

    public void RemoveItem(OrderMenuItemModel item) {
        var existingItem = OrderItems.FirstOrDefault(x => x.ID == item.ID);
        if (existingItem == null) return;
        existingItem.Count -= item.Count;
        if (existingItem.Count <= 0) {
            OrderItems.Remove(existingItem);
        }
    }
}