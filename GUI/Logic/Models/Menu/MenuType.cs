using System;
using Logic.Models.Base;

namespace GUI.Logic.Models.Menu;

public class MenuType : BaseType {
    public int ID;
    public string Name;
    public decimal Price;
    public string Type;

    public MenuType(int id, string name, string type, decimal price) : base(id) {
        Name = name;
        Price = price;
        Type = type;
    }



    public override T getByID<T>(int id) {
        throw new NotImplementedException();
    }
}