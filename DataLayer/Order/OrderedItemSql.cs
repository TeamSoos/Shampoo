using ModelLayer.OrderModel;
using Npgsql;

namespace DataLayer.Order; 

public class OrderedItemSql : BaseSQL<OrderedItem> {
    public void add_ordered_item(OrderedItem item) {
        var cmd = new NpgsqlCommand("INSERT INTO ordered_items (order_id, item_id, quantity, note) VALUES (@order_id, @item_id, @quantity, @note)");
        cmd.Parameters.AddWithValue("order_id", item.OrderID);
        cmd.Parameters.AddWithValue("item_id", item.ItemID);
        cmd.Parameters.AddWithValue("quantity", item.Quantity);
        cmd.Parameters.AddWithValue("note", item.Note ?? "");

        Store(cmd);
    }
    
    protected override OrderedItem ReadTables(NpgsqlDataReader reader) {
        return new OrderedItem() {
            ID = (int)reader["id"],
            OrderID = (int)reader["order_id"],
            ItemID = (int)reader["item_id"],
            Quantity = (int)reader["quantity"],
            Note = (string)reader["note"]
        };
    }
}