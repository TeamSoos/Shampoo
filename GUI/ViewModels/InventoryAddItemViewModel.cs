using System;
using System.Reactive;
using Avalonia.Notification;
using GUI.Views;
using Logic.Models.Item;
using ReactiveUI;
using ServiceLayer.OrderMenu;

namespace GUI.ViewModels;

public class InventoryAddItemViewModel : RoutablePage {

    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> BackButton { get; set; }
    
    public ReactiveCommand<Unit, Unit> AddButton { get; set; }
    
    public string ItemID { get; set; }
    public string ItemQuantity { get; set; }

    public MenuItemService _service;

    public InventoryAddItemViewModel(IHostScreen screen) {
        HostScreen = screen;
        BackButton = ReactiveCommand.Create(() =>
        {
            HostScreen.GoBack();
        });
        
        AddButton = ReactiveCommand.Create(() =>
        {
            int.TryParse(ItemID, out int id);
            int.TryParse(ItemQuantity, out int quantity);

            if (id == 0 || quantity == 0)
            {
                HostScreen.Notify( "Invalid input!", 5);
            }
            
            _service.UpdateCount(
                _service.GetById(id),
                quantity
            );
            HostScreen.GoBack();
            
            HostScreen.Notify( "Item added!", 5);
        });
    }
}