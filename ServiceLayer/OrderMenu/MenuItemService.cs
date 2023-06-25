using DataLayer.OrderMenu;
using ModelLayer.OrderMenu;

namespace ServiceLayer.OrderMenu;

public class MenuItemService {
    public async Task<List<OrderMenuItem>> GetItemsOfMenu(OrderMenuItem.EMenuType type) {
        OrderItemSQL sql = new OrderItemSQL();
        var result = await sql.get_of_menu(type);
        return result;
    }
}