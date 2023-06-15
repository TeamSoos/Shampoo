using System.IO;
using ReactiveUI;
using RoutedApp.ViewModels;

namespace GUI.ViewModels;

public class PaymentsViewModel : RoutablePage {
    public override IHostScreen HostScreen { get; }

    private string _waiter;
    private string _total;
    private string _tableNr;

    public string Waiter
    {
        get { return _waiter;}
        set
        {
            this.RaiseAndSetIfChanged(ref _waiter, value); // notifies the whole app that this value is changes
        }
    }

    public string Total
    {
        get { return _total;}
        set
        {
            this.RaiseAndSetIfChanged(ref _total, value);
        }
    }

    public string TableNr
    { 
        get { return _tableNr;}
        set
        {
            this.RaiseAndSetIfChanged(ref _total, value);
        }
    }
    
    
public PaymentsViewModel(IHostScreen screen)
    {
        HostScreen = screen;
    }
}