using Logic.Models.Base;

namespace GUI.Logic.Models.Order; 

public class OrderType : BaseType {
    public int OrderId;
    public int MenuItemId;
    public int Quantity;
    public bool Paid;
    public bool Delivered;

    public OrderType(int id, bool paid, bool delivered) : base(id) {
        OrderId = id;
        Paid = paid;
        Delivered = delivered;
    }

    public static void Deliver(int id) {
        OrderSql.deliver_by_id(id);
    }

    public override T getByID<T>(int id) {
        throw new System.NotImplementedException();
    }
}