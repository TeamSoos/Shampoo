using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using GUI.Logic.Models.Menu;
using ReactiveUI;

namespace GUI.ViewModels;

public class OrderMenuViewModel : RoutablePage {


    public List<Menu> _listItems;
    readonly string activebtnc = "#B5ECA1";

    string colour1;
    string colour2;
    string colour3;

    readonly string defaultbtnc = "#B4BEFE";
    int Table;
    string Waiter;

    public OrderMenuViewModel(IHostScreen screen, string waiter, int table) {
        Waiter = waiter;
        Table = table;

        // List items for the menu
        _listItems = new List<Menu>();


        HostScreen = screen;
        colour1 = defaultbtnc;
        colour2 = defaultbtnc;
        colour3 = defaultbtnc;

        GoBack = ReactiveCommand.Create(() => { HostScreen.GoBack(); });

        viewOrder = ReactiveCommand.Create(() => { HostScreen.GoNext(new OrderMenuViewOrderViewModel(HostScreen)); });

        getLunch = ReactiveCommand.Create(() => {
            // default buttons
            Colour2 = defaultbtnc;
            Colour3 = defaultbtnc;
            // active button
            Colour1 = activebtnc;

            SetCurrentMenu(EMenuType.Lunch);

        });
        getDinner = ReactiveCommand.Create(() => {
            // default buttons
            Colour1 = defaultbtnc;
            Colour3 = defaultbtnc;
            // active button
            Colour2 = activebtnc;

            SetCurrentMenu(EMenuType.Dinner);
        });

        getDrinks = ReactiveCommand.Create(() => {
            // default buttons
            Colour2 = defaultbtnc;
            Colour1 = defaultbtnc;
            // active button
            Colour3 = activebtnc;

            SetCurrentMenu(EMenuType.Drinks);
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
    public ReactiveCommand<Unit, Unit> GoBack { get; }

    public List<Menu> ListItems {
        get => _listItems;
        set => this.RaiseAndSetIfChanged(ref _listItems, value);
    }

    void SetCurrentMenu(EMenuType type) {
        ListItems = new List<Menu> {
            new Menu("loading...", new List<MenuItem>())
        };

        var getAllTask = MenuSql.get_all(type);
        getAllTask.GetAwaiter().OnCompleted(() => {
            var items = getAllTask.Result;

            var TypedMenuItems = items
                    .GroupBy(item => item.Type)
                    .Select(group => {
                        var MenuItems = group.Select(x => new MenuItem(x, HostScreen)).ToList();
                        var menu = new Menu(group.Key, MenuItems);
                        return menu;
                    })
                    .ToList()
                ;

            ListItems = TypedMenuItems;
        });
    }

}

public class Menu {

    public Menu(string heading, List<MenuItem> items) {
        Heading = heading;
        MenuStuff = new List<MenuItem>(items);
    }


    public string Heading { get; }

    public List<MenuItem> MenuStuff { get; }
}