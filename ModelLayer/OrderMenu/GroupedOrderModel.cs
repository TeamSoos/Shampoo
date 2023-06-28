namespace ModelLayer.OrderMenu;

public class GroupedMenuModel {
    public string Heading { get; set; } = "";
    public List<OrderMenuItemModel> Menu { get; set; } = new();

    // we now need to convert a List of OrderMenuModel to a list of this

    public static List<GroupedMenuModel> FromList(List<OrderMenuItemModel> list) {
        return list
            .GroupBy(item => item.Type)
            .Select(group => new GroupedMenuModel {
                Heading = group.Key,
                Menu = group.ToList()
            })
            .ToList();
    }
}