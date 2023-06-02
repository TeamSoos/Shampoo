using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class OrderingViewModel : RoutablePage {
  private ItemListViewModel _currentOrderItemsList;
  public ItemListViewModel CurrentOrderItemsList {
    get => _currentOrderItemsList;
    set => this.RaiseAndSetIfChanged(ref _currentOrderItemsList, value);
  }
  private CardListViewModel _nextOrdersCardList;
  public CardListViewModel NextOrdersCardList {
    get => _nextOrdersCardList;
    set => this.RaiseAndSetIfChanged(ref _nextOrdersCardList, value);
  }
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING

  public OrderingViewModel(IHostScreen screen) { 
      HostScreen = screen;
    CurrentOrderItemsList = new ItemListViewModel(
        new List<ListItem>() {
            new() { Title = "Bread", Notes = "notes: no bread", Status = "Orange"},
        }
    );
    NextOrdersCardList = new CardListViewModel(
        new List<CardItem>() {
            new() { Title = "Order nm 01", Description = @"1x - steak 1x - deer soup 1x - coca cola   notes: wants extra bread" },
            new() { Title = "Card 2", Description = "Description 2" }
        }
    );
    NextOrdersCardList.Items.Add(
        new() {
            Title = "Donuts", Description = "Holy shit is that a Simpsons reference"
        }
    );
  }
  
  public override IScreen HostScreen { get; }
}