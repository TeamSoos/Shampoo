using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Logic.Models.Item;
using ReactiveUI;
using GUI.ViewModels;
using ModelLayer.OrderMenu;
using ServiceLayer.OrderMenu;

namespace GUI.ViewModels;

public class InventoryItemsListViewModel: RoutablePage
{
    public override IHostScreen HostScreen { get; }
    public MenuItemService _service = new MenuItemService();
    public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }
    public ReactiveCommand<Unit, Unit> AddStock { get; set; }
    public ReactiveCommand<Unit, Unit> AddItem { get; set; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    private ItemListViewModel _allItem;
    public ItemListViewModel AllItems {
        get => _allItem;
        set => this.RaiseAndSetIfChanged(ref _allItem, value);
    }

    public InventoryItemsListViewModel(IHostScreen screen)
    {
        HostScreen = screen;
        GoBack = ReactiveCommand.Create(screen.GoBack);
        LogoutUser = ReactiveCommand.Create(
            () =>
            {
                screen.GoNext(new LoginPageViewModel(screen));
            }
            );
        AddStock =ReactiveCommand.Create(
            () =>
            {
                screen.GoNext(new InventoryAddItemViewModel(screen));
            }
        );
        AddItem =ReactiveCommand.Create(
            () =>
            {
                screen.GoNext(new AddItemViewModel(screen));
            }
        );
        AllItems = new ItemListViewModel(
            new List<ListItem>()
        );
        _service = new MenuItemService();
        reloadItems();
    }

    private void reloadItems()
    {
        AllItems.Items.Clear();

        List<OrderMenuItemModel> items = new List<OrderMenuItemModel>();
        items = _service.GetAll();

        foreach (OrderMenuItemModel item in items)
        {
            AllItems.Items.Add(
                new ListItem()
                {
                    Count = item.Count,
                    Title = item.Name,
                    Notes = $"ID: {item.ID}"
                }
            );
        }
    }

    public override void OnLoad() 
    {
        reloadItems();
    }
}