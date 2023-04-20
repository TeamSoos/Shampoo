using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.Views; 

public partial class BasicAppView : UserControl {
  public BasicAppView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }
}