using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using GUI.Logic.Models.Menu;
using ModelLayer.OrderMenu;
using ReactiveUI;
using ServiceLayer.OrderMenu;

namespace GUI.ViewModels;

public class OrderMenuViewModel : RoutablePage {
    public List<GroupedMenuModel> _listItems;
    readonly string activebtnc = "#B5ECA1";

    MenuItemService service = new();

    string colour1;
    string colour2;
    string colour3;

    readonly string defaultbtnc = "#B4BEFE";

    public void ViewItemInfo(OrderMenuItemModel item) {
        HostScreen.GoNext(new OrderItemInfoViewModel(HostScreen, item));
    }

    public OrderMenuViewModel(IHostScreen screen) {
        // List items for the menu
        _listItems = new();
        screen.CurrentOrder = new();


        HostScreen = screen;
        colour1 = defaultbtnc;
        colour2 = defaultbtnc;
        colour3 = defaultbtnc;

        GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        ShowItemInfo = ReactiveCommand.Create<OrderMenuItemModel>(ViewItemInfo);

        viewOrder = ReactiveCommand.Create(() => {
            var filteredItems = service.FilterUnorderedItems(listItemsUngrouped!);
            filteredItems.ForEach(item => {
                HostScreen.CurrentOrder.OrderItems.Add(item);
            });
            listItemsUngrouped!.Clear();
            HostScreen.GoNext(
                new OrderMenuViewOrderViewModel(HostScreen));
        });

        getLunch = ReactiveCommand.Create(() => {
            // default buttons
            Colour2 = defaultbtnc;
            Colour3 = defaultbtnc;
            // active button
            Colour1 = activebtnc;

            SetMenuOnDisplay(OrderMenuItemModel.EMenuType.Lunch);

        });
        getDinner = ReactiveCommand.Create(() => {
            // default buttons
            Colour1 = defaultbtnc;
            Colour3 = defaultbtnc;
            // active button
            Colour2 = activebtnc;

            SetMenuOnDisplay(OrderMenuItemModel.EMenuType.Dinner);
        });

        getDrinks = ReactiveCommand.Create(() => {
            // default buttons
            Colour2 = defaultbtnc;
            Colour1 = defaultbtnc;
            // active button
            Colour3 = activebtnc;

            SetMenuOnDisplay(OrderMenuItemModel.EMenuType.Drinks);
        });

        getLunch.Execute().Subscribe();
    }

    public override IHostScreen HostScreen { get; }

    public string Colour1 {
        get => colour1;
        set => this.RaiseAndSetIfChanged(ref colour1, value);
    }

    public string Colour2 {
        get => colour2;
        set => this.RaiseAndSetIfChanged(ref colour2, value);
    }

    public string Colour3 {
        get => colour3;
        set => this.RaiseAndSetIfChanged(ref colour3, value);
    }


    public ReactiveCommand<Unit, Unit> getLunch { get; }
    public ReactiveCommand<Unit, Unit> getDinner { get; }
    public ReactiveCommand<Unit, Unit> getDrinks { get; }
    public ReactiveCommand<Unit, Unit> viewOrder { get; }
    public ReactiveCommand<OrderMenuItemModel, Unit> ShowItemInfo { get; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }

    public List<GroupedMenuModel> ListItems {
        get { return _listItems; }
        set => this.RaiseAndSetIfChanged(ref _listItems, value);
    }

    List<OrderMenuItemModel> listItemsUngrouped;

    void SetMenuOnDisplay(OrderMenuItemModel.EMenuType type) {
        listItemsUngrouped = service.GetItemsOfMenuSync(type);
        ListItems = service.GetFromItemList(listItemsUngrouped);
    }
}