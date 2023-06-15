using System;
using Logic.Models.Base;

namespace GUI.Logic.Models.Menu;

public class MenuType : BaseType {
    public int ID;
    public string Name;
    public decimal Price;
    public string Type;

    MenuType(int id) : base(id) {
    }



    public override T getByID<T>(int id) {
        throw new NotImplementedException();
    }
}