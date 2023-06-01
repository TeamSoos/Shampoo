using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.Views; 

public partial class OrderingView : UserControl {
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING
  public OrderingView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }
}