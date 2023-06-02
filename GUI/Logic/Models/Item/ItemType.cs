using System;
using Logic.Models.Base;

namespace Logic.Models.Item; 

public class ItemType : BaseType {

  public string Name;

  public ItemType(int id, string name) : base(id) {
    Name = name;
  }
  public override T getByID<T>(int id) {
    throw new NotImplementedException();
  }
}