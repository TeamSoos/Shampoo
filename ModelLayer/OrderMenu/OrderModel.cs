namespace ModelLayer.OrderMenu;

public class OrderModel {
    public int ID;
    public bool Paid { get; } // gets directly from the database
    public bool Delivered { get; set; } // gets directly from the database
    

    List<OrderMenuItemModel> _orderItems = new();

    public List<OrderMenuItemModel> OrderItems {
        get => _orderItems;
        protected set { _orderItems = ManyToOne(value); }
    }

    /// <summary>
    /// Might be depracated...
    /// If we have multiple entries of the same item, we would group them
    /// This is in place as a small little failsafe
    /// </summary>
    /// <param name="order"></param>
    /// <returns><see cref="List{T}"/></returns>
    private List<OrderMenuItemModel> ManyToOne(List<OrderMenuItemModel> order) {
        return order.GroupBy(item => item.ID)
            .Select(group => {
                int count = group.Sum(item => item.Count);

                var item = group.First();
                item.Count = count;

                return item;
            })
            .ToList();

    }


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