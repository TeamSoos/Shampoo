using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Notification;
using ReactiveUI;

namespace GUI.ViewModels;

public class KitchenViewModel : RoutablePage {
    ItemListViewModel _currentOrderItemsList;

    CardListViewModel _nextOrdersCardList;
    // END STYLING

    public KitchenViewModel(IHostScreen screen) {
        LogoutUser = ReactiveCommand.Create(logoutUserAction);
        HostScreen = screen;
        CurrentOrderItemsList = new ItemListViewModel(
            new List<ListItem> {
                new ListItem { Title = "Bread", Notes = "notes: no bread", Status = "Orange" }
            }
        );
        NextOrdersCardList = new CardListViewModel(
            new List<CardItem> {
                new CardItem {
                    Title = "Order nm 01", Description = @"1x - steak 1x - deer soup 1x - coca cola   notes: wants extra bread"
                },
                new CardItem { Title = "Card 2", Description = "Description 2" }
            }
        );
        NextOrdersCardList.Items.Add(
            new CardItem {
                Title = "Donuts", Description = "Holy shit is that a Simpsons reference"
            }
        );
    }

    public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }

    public ItemListViewModel CurrentOrderItemsList {
        get => _currentOrderItemsList;
        set => this.RaiseAndSetIfChanged(ref _currentOrderItemsList, value);
    }

    public CardListViewModel NextOrdersCardList {
        get => _nextOrdersCardList;
        set => this.RaiseAndSetIfChanged(ref _nextOrdersCardList, value);
    }

    // Styling
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;


    public override IHostScreen HostScreen { get; }

    public void logoutUserAction() {
        HostScreen.notificationManager.CreateMessage()
            .Animates(true)
            .Background("#B4BEFE")
            .Foreground("#1E1E2E")
            .HasMessage(
                $"Logging out. Good bye {HostScreen.CurrentUser}")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
        HostScreen.GoNext(new LoginPageViewModel(HostScreen));
    }
}