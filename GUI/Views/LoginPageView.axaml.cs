using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace GUI.Views; 

public class GradientConverter : Avalonia.Data.Converters.IValueConverter
{
  public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    var rect = (Rect)value;

    var startColor = Color.Parse("#1E1E2E");
    var endColor = Color.Parse("#45475A");

    var brush = new LinearGradientBrush
    {
        StartPoint = new RelativePoint(rect.Width / 2, 0, RelativeUnit.Absolute),
        EndPoint = new RelativePoint(rect.Width / 2, rect.Height, RelativeUnit.Absolute),
        GradientStops = new GradientStops()
        {
            new GradientStop(startColor, 0),
            new GradientStop(endColor, 1)
        }
    };

    return brush;
  }

  public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    throw new System.NotImplementedException();
  }
}
public partial class LoginPageView : UserControl {
  public LoginPageView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
  }
}