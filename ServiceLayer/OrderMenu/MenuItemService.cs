using DataLayer.OrderMenu;
using ModelLayer.OrderMenu;

namespace ServiceLayer.OrderMenu;

public class MenuItemService {
    private OrderItemSQL sql = new OrderItemSQL();

    public List<OrderMenuItemModel> GetItemsOfMenu(OrderMenuItemModel.EMenuType type) {
        var result =  sql.get_of_menu(type);
        return result;
    }

    // sync variant for UI load
    public List<OrderMenuItemModel> GetItemsOfMenuSync(OrderMenuItemModel.EMenuType type) {
        return GetItemsOfMenu(type);
    }

    public List<GroupedMenuModel> GetFromItemList(List<OrderMenuItemModel> list) {
        return GroupedMenuModel.FromList(list);
    }

    public List<OrderMenuItemModel> FilterUnorderedItems(List<OrderMenuItemModel> items) {
        return items
            .Where(model => model.OrderedCount > 0)
            .ToList();
    }
    
    public void AddItemsToMenu(OrderMenuItemModel item)
    {
        sql.add_to_menu(item);
    }

    public OrderMenuItemModel GetById(int id)
    {
        return sql.get_by_id(id);
    }

    public List<OrderMenuItemModel> GetAll()
    {
        return sql.get_all();
    }

    public void UpdateCount(OrderMenuItemModel item, int count)
    {
        sql.add_stock(item, count);
    }
}