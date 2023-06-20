using System;
using System.Reactive;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Logic.Models.Menu; 
public class MenuItem {

    public MenuItem(MenuType menuType, IHostScreen screen) {
        Name = menuType.Name;
        Command = ReactiveCommand.Create(() => {
            screen.GoNext(new OrderItemInfoViewModel(screen, menuType, AddToOrder, AddNote));
        });
        HostScreen = screen;
        Price = menuType.Price;
        MenuItemFull = menuType;
    }

    void AddToOrder(int amount) {
        while (amount > 0) {
            HostScreen.CurrentOrder.Add(this);
            amount--;
        }
    }

    void AddNote(string note) {
        
    }

    
    /// <summary>
    /// This one is here for SQL queries. I have unpacked some other values for ease of use in the GUI though...
    /// </summary>
    public MenuType MenuItemFull { get; set; }
    public string Name { get; }
    public string Note { get; set; } = "";
    public decimal Price { get; set; }
    IHostScreen HostScreen;

    public ReactiveCommand<Unit, Unit> Command { get; }
}
