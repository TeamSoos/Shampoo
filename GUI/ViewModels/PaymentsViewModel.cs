using System.Collections.Generic;
using System.IO;
using System.Reactive;
using ReactiveUI;
using GUI.ViewModels;
using GUI.Views;
using RoutedApp.Logic.Models.Payment;

namespace GUI.ViewModels;

public class PaymentsViewModel : RoutablePage {
    public override IHostScreen HostScreen { get; }

    private string _waiter;
    private string _total;
    private string _tableNr;

    public ReactiveCommand<Unit, Unit> PayScreen { get; set; }
    
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }
    


    public string Waiter
    {
        get { return $"Waiter: {_waiter}";}
        set
        {
            this.RaiseAndSetIfChanged(ref _waiter, value); // notifies the whole app that this value is changes
        }
    }

    public string Total
    {
        get { return $"Total: €{_total}";}
        set
        {
            this.RaiseAndSetIfChanged(ref _total, value);
        }
    }

    public string TableNr
    {
        get { return $"Table No: {_tableNr}";}
        set
        {
            this.RaiseAndSetIfChanged(ref _total, value);
        }
    }

    public PaymentsViewModel(IHostScreen screen)
    {
        HostScreen = screen;

        GoBack = ReactiveCommand.Create(() =>
        {
            screen.GoBack();
        });

        PayScreen = ReactiveCommand.Create((() =>
        {
            screen.GoNext(new FinalPaymentViewModel(screen));
        }));
        int table = screen.CurrentTable.ID;
        Waiter = screen.CurrentUser.Name;
        
        var res = PaymentSQL.get_total(table);
        TableNr = table.ToString();
        res.GetAwaiter().OnCompleted(() => 
        {
            Total = res.Result.ToString("00.00"); 
        });
    }
}