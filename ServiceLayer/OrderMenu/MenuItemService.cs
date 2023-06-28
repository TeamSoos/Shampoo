using DataLayer.OrderMenu;
using ModelLayer.OrderMenu;

namespace ServiceLayer.OrderMenu;

public class MenuItemService {
    OrderItemSQL sql = new OrderItemSQL();
    public async Task<List<OrderMenuItemModel>> GetItemsOfMenu(OrderMenuItemModel.EMenuType type) {
        var result = await sql.get_of_menu(type);
        return result;
    }







    public void AddItemToMenu(OrderMenuItemModel item)
    {
        sql.add_to_menu(item);
    }
}