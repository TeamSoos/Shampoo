using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Logic.Models.Base;

namespace Logic.Models.Item; 

public class ItemType
{

  public int Id;
  public string Name;
  public int Count;

  private ItemType(int id, string name, int count)
  {
    this.Count = count;
    this.Name = name;
    this.Id = id;
  }
  
  public ItemType(int id) {
    Task.Run(async () =>
    {
      Dictionary<string, dynamic> table_data = await ItemSQL.get_by_id(id);
      Id = id;
      Name = table_data["name"];
      Count = table_data["count"];
    }).Wait();
  }

  public static ItemType raw(int id, string name, int count)
  {
    return new ItemType(id, name, count);
  }

  public async Task<List<ItemType>>  GetAll()
  {
    return await ItemSQL.GetAll();
  }

}