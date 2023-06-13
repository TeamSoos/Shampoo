﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Logic.Models.Item;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class InventoryItemsListViewModel: RoutablePage
{
    public override IHostScreen HostScreen { get; }
    
    public ReactiveCommand<Unit, Unit> LogoutUser { get; set; }
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
        AddItem =ReactiveCommand.Create(
            () =>
            {
                screen.GoNext(new InventoryAddItemViewModel(screen));
            }
        );
        AllItems = new ItemListViewModel(
            new List<ListItem>()
        );
        reloadItems();
    }

    private async void reloadItems()
    {
        AllItems.Items.Clear();
        foreach (ItemType item in await ItemSQL.GetAll())
        {
            AllItems.Items.Add(
                new ListItem()
                {
                    Count = item.Count,
                    Title = item.Name,
                    Notes = $"ID: {item.Id}"
                }
            );
        }
    }
}