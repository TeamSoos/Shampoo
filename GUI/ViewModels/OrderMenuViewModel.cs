using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Notification;
using Avalonia.Notification.Controls;
using Npgsql;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class OrderMenuViewModel : RoutablePage {
	public override IHostScreen HostScreen { get; }

	string defaultbtnc = "#B4BEFE";
	string activebtnc = "#B5ECA1";

	string colour1;
	string colour2;
	string colour3;

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

	public List<Menu> _listItems;

	public List<Menu> ListItems {
		get { return _listItems; }
		set { this.RaiseAndSetIfChanged(ref _listItems, value); }
	}

	public OrderMenuViewModel(IHostScreen screen) {

		// List items for the menu
		_listItems =
			new List<Menu> {
				new Menu("ANTHREEE",
					new List<MenuItem> {
						new MenuItem("balz"),
						new MenuItem("ssssssss")
					}
				)
			};


		HostScreen = screen;
		colour1 = defaultbtnc;
		colour2 = defaultbtnc;
		colour3 = defaultbtnc;


		viewOrder = ReactiveCommand.Create(() => {
			HostScreen.GoNext(new OrderMenuViewOrderViewModel(HostScreen, ListItems));
		});

		getLunch = ReactiveCommand.Create(() => {
			// default buttons
			Colour2 = defaultbtnc;
			Colour3 = defaultbtnc;
			// active button
			Colour1 = activebtnc;

		});
		getDinner = ReactiveCommand.Create(() => {
			// default buttons
			Colour1 = defaultbtnc;
			Colour3 = defaultbtnc;
			// active button
			Colour2 = activebtnc;

			ListItems =
				new List<Menu> {
					new Menu("SOUPPPPPPPP",
						new List<MenuItem> {
							new MenuItem("mineral soup from the nile river"),
							new MenuItem("blood")
						}
					),
					new Menu("SOUP",
						new List<MenuItem> {
							new MenuItem("Tomato"),
							new MenuItem("Tomato"),
						}
					)
				};

		});
		getDrinks = ReactiveCommand.Create(() => {
			// default buttons
			Colour2 = defaultbtnc;
			Colour1 = defaultbtnc;
			// active button
			Colour3 = activebtnc;
			//  HostScreen.notificationManager.CreateMessage()
			//  .Animates(true)
			//  .HasMessage("Failed to switch")
			//  .Background("#B4BEFE")
			//  .Foreground("#1E1E2E")
			//  .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
			//  .Queue();
		});

		getLunch.Execute().Subscribe();
	}
}

public class Menu {
	string _heading;
	List<MenuItem> _items;


	public string Heading => _heading;
	public List<MenuItem> MenuStuff => _items;

	public Menu(string heading, List<MenuItem> items) {
		this._heading = heading;
		this._items = new List<MenuItem>(items);
	}
}

public class MenuItem {
	string _name;

	public string Name => _name;
	public ReactiveCommand<Unit, Unit> Command { get; }

	public MenuItem(string name) {
		this._name = name;
		Command = ReactiveCommand.Create(() => { Console.WriteLine($"{_name}"); });
	}
}

// DATABASE CODE
// I have this code here as it makes it so much easier to iterate

class MenuData {
	int ID;
	string Type;
	string Name;
	decimal Price;
	
	private MenuData(int id, string type, string name, decimal price) {
		ID = id;
		Type = type;
		Name = name;
		Price = price;
	}

	public static List<MenuData> GetMenu(string menu) {
		switch (menu) {
			case "lunch":
				break;
			case "dinner":
				break;
			case "drink":
				break;
			default:
				throw new Exception("Unknown");
		}

		return new List<MenuData>();

	}
	
}