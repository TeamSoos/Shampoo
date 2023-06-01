using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;

namespace GUI.ViewModels;

public class CardItem {
  public string Title { get; set; }
  public string Description { get; set; }
}



public class ListItem {
  private Color _status;
  public string Status { get; set; }
  public int Count { get; set; }
  public string Title { get; set; }
  public string Notes { get; set; }
}

public class ItemListViewModel : ViewModelBase {
  public ItemListViewModel(IEnumerable<ListItem> items)
  {
    Items = new ObservableCollection<ListItem>(items);
  }

  public ObservableCollection<ListItem> Items { get; set; }
}

public class CardListViewModel : ViewModelBase {
  public CardListViewModel(IEnumerable<CardItem> items)
  {
    Items = new ObservableCollection<CardItem>(items);
  }

  public ObservableCollection<CardItem> Items { get; set; }
}