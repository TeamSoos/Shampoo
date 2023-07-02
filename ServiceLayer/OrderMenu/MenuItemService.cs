using DataLayer.OrderMenu;
using ModelLayer.OrderMenu;

namespace ServiceLayer.OrderMenu;

public class MenuItemService {
    private OrderItemSQL sql = new OrderItemSQL();

    public async Task<List<OrderMenuItemModel>> GetItemsOfMenu(OrderMenuItemModel.EMenuType type) {
        var result = await sql.get_of_menu(type);
        return result;
    }

    // sync variant for UI load
    public List<OrderMenuItemModel> GetItemsOfMenuSync(OrderMenuItemModel.EMenuType type) {
        List<OrderMenuItemModel> list = new();

        Task.Run(async () => { list = await GetItemsOfMenu(type); }).Wait();

        return list;
    }

    public List<GroupedMenuModel> GetFromItemList(List<OrderMenuItemModel> list) {
        return GroupedMenuModel.FromList(list);
    }

    public List<OrderMenuItemModel> FilterUnorderedItems(List<OrderMenuItemModel> items) {
        return items
            .Where(model => model.OrderedCount > 0)
            .ToList();
    }
}