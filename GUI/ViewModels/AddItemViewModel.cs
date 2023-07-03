using System.Reactive;
using Avalonia.Controls;
using ModelLayer;
using ModelLayer.OrderMenu;
using ReactiveUI;
using ServiceLayer.OrderMenu;

namespace GUI.ViewModels;

public class AddItemViewModel : RoutablePage
{

    public int MenuTypeIndex { get; set; } = -1;
    public ComboBoxItem MenuTypeString { get; set; }


    public string ItemName { get; set; } = "";
    public string ItemCat { get; set; } = "";
    
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public ReactiveCommand<Unit, Unit> AddItem { get; }

    public AddItemViewModel(IHostScreen hostScreen)
    {
        MenuItemService _service = new();
        GoBack = ReactiveCommand.Create(hostScreen.GoBack);
        AddItem = ReactiveCommand.Create(() =>
        {
            OrderMenuItemModel newItem = new OrderMenuItemModel()
            {
                Alcoholic = IsAlcoholic,
                Count = 0,
                Price = decimal.Parse(ItemPrice!),
                ID = -1,
                Menu = (OrderMenuItemModel.EMenuType)MenuTypeIndex,
                Name = ItemName,
                Type = ItemCat
            };
            _service.AddItemsToMenu(newItem);
            hostScreen.Notify("Added item", 2);
        });
        HostScreen = hostScreen;
    }

    public override IHostScreen HostScreen { get; }
    public string ItemPrice { get; set; }
    public bool isAlcoholic;

    public bool IsAlcoholic
    {
        get => isAlcoholic;
        set => this.RaiseAndSetIfChanged(ref isAlcoholic, value);
    }
}