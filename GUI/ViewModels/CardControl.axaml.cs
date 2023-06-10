using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.ViewModels; 

public partial class CardControl : UserControl {
  public readonly static AvaloniaProperty<string> TitleProperty =
      AvaloniaProperty.Register<CardControl, string>(nameof(Title));

  public string Title
  {
    get => GetValue(TitleProperty) as string;
    set => SetValue(TitleProperty, value);
  }

  public readonly static AvaloniaProperty<string> DescriptionProperty =
      AvaloniaProperty.Register<CardControl, string>(nameof(Description));

  public string Description
  {
    get => GetValue(DescriptionProperty) as string;
    set => SetValue(DescriptionProperty, value);
  }

  public CardControl()
  {
    InitializeComponent();
  }

  private void InitializeComponent()
  {
    AvaloniaXamlLoader.Load(this);
  }
}