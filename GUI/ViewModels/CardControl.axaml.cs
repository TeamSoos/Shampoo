using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.ViewModels;

public partial class CardControl : UserControl {
    public static readonly AvaloniaProperty<string> TitleProperty =
        AvaloniaProperty.Register<CardControl, string>(nameof(Title));

    public static readonly AvaloniaProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<CardControl, string>(nameof(Description));

    public CardControl() {
        InitializeComponent();
    }

    public string Title {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public string Description {
        get => GetValue(DescriptionProperty) as string;
        set => SetValue(DescriptionProperty, value);
    }

    void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}