using DataLayer.Tables;
using ModelLayer.Tables;

namespace ServiceLayer.Tables; 

public class TablesService {
  private TablesSQL _sql;

  public TablesService() {
    _sql = new TablesSQL();
  }
  
  public async Task<Table> GetOne(int tableID) {
    return await _sql.get_one(tableID);
  }

  public async Task<List<Table>> GetAllAsync() {
    return await _sql.get_all_async();
  }

  public List<Table> GetAll() {
    return _sql.get_all();
  }
  
  public void Reserve(Table table) {
    _sql.Reserve(table);
  }
  
  public void Free(Table table) {
    _sql.Free(table);
  }
  
  public void Occupy(Table table) {
    _sql.Occupy(table);
  }
}