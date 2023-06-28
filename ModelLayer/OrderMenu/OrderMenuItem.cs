namespace ModelLayer.OrderMenu;

public class OrderMenuItem {
    public int ID { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Type { get; set; } = "";
    public int Count { get; set; }
    public EMenuType Menu { get; set; }

    public enum EMenuType {
        Drinks,
        Lunch,
        Dinner,
    }

    // STATIC CLASS FOR UTILITY
    public static Dictionary<string, List<OrderMenuItem>> ItemsListGrouped(IEnumerable<OrderMenuItem> Items) {
        return Items
            .GroupBy(x => x.Type)
            .ToDictionary(
                x => x.Key,
                x => x.ToList()
            );
    }

    public static EMenuType MenuFromString(string menu) {
        return menu switch {
            "drinks" => EMenuType.Drinks,
            "lunch" => EMenuType.Lunch,
            _ => EMenuType.Dinner, // if something fucks up somehow, its dinner
        };
    }

    public static string StringFromMenu(EMenuType menu) {
        return menu switch {
            EMenuType.Drinks => "drinks",
            EMenuType.Lunch => "lunch",
            _ => "dinner", // if something fucks up somehow, its dinner
        };
    }
}