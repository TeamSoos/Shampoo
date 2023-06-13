using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.Views; 


public partial class LoginPageView : ReactiveUserControl<LoginPageViewModel> {
  private TextBlock staffLoginBox;
  // Styling
  public int FormContentWidth => 350;
  public int FormContentSpacing => 10;
  // END STYLING

  public LoginPageView() {
    InitializeComponent();
  }

  private void InitializeComponent() {
		this.WhenActivated(_ => { });
    AvaloniaXamlLoader.Load(this);
    staffLoginBox = this.FindControl<TextBlock>("StaffLoginBox");
  }

  public void LoginStaff() {
    Console.WriteLine(staffLoginBox.Text);
  }
}