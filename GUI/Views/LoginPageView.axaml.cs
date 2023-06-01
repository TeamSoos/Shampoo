using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace GUI.Views; 


public partial class LoginPageView : UserControl {
  private TextBlock staffLoginBox;
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING

  public LoginPageView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
    AvaloniaXamlLoader.Load(this);
    staffLoginBox = this.FindControl<TextBlock>("StaffLoginBox");
  }

  public void LoginStaff() {
    Console.WriteLine(staffLoginBox.Text);
  }
}