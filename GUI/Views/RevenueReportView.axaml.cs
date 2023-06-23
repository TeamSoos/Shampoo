using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GUI.ViewModels;

namespace GUI.Views;

public partial class RevenueReportView : ReactiveUserControl<RevenueReportViewModel>
{
    public RevenueReportView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}