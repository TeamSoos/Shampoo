using DataLayer.Order;
using ModelLayer.Tables;

namespace ServiceLayer.Order; 

public class OrderService {
  private OrderSQL _sql;

  public OrderService() {
    _sql = new OrderSQL();
  }

  public ModelLayer.Order GetOne(int orderID) {
    return _sql.get_one(orderID);
  }

  public List<ModelLayer.Order> GetAllByTable(Table table) {
    return _sql.get_all_by_table(table);
  }
}