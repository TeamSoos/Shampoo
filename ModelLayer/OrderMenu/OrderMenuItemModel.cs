namespace ModelLayer.OrderMenu;

public class OrderMenuItemModel {
    public int ID { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Type { get; set; } = "";
    public int Count { get; set; }
    public EMenuType Menu { get; set; }
    public bool Alcoholic { get; set; }

    public string? Note { get; set; } = "";
    private int _orderedCount = 0;

    public int OrderedCount {
        get => _orderedCount;
        set => _orderedCount = value <= Count ? value : Count;
    }

    public enum EMenuType {
        Lunch,
        Dinner,
        Drinks,
    }

    // STATIC CLASS FOR UTILITY
    public static Dictionary<string, List<OrderMenuItemModel>> ItemsListGrouped(IEnumerable<OrderMenuItemModel> Items) {
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