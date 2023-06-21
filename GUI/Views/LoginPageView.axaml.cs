using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public partial class LoginPageView : ReactiveUserControl<LoginPageViewModel> {
    TextBlock staffLoginBox;
    // END STYLING

    public LoginPageView() {
        InitializeComponent();
    }

    // Styling
    public int FormContentWidth => 350;
    public int FormContentSpacing => 10;

    void InitializeComponent() {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
        staffLoginBox = this.FindControl<TextBlock>("StaffLoginBox");
    }

    public void LoginStaff() {
        Console.WriteLine(staffLoginBox.Text);
    }
}