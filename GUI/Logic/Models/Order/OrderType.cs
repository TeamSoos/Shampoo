using Logic.Models.Base;

namespace GUI.Logic.Models.Order; 

public class OrderType : BaseType {
    public int OrderId;
    public int MenuItemId;
    public int Quantity;

    public OrderType(int id) : base(id) {
    }

    public override T getByID<T>(int id) {
        throw new System.NotImplementedException();
    }
}