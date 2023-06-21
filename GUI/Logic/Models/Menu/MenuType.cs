using System;
using Logic.Models.Base;

namespace GUI.Logic.Models.Menu;

public class MenuType : BaseType {
    public int ID;
    public string Name;
    public decimal Price;
    public string Type;
    public int Count;

    public MenuType(int id, string name, string type, decimal price, int count) : base(id) {
        Name = name;
        Price = price;
        Type = type;
        ID = id;
        Count = count;
    }



    public override T getByID<T>(int id) {
        throw new NotImplementedException();
    }
}