using System.Reactive;
using ExCSS;
using GUI.Logic.Models.Order;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.Views;

public class FinalPaymentViewModel : RoutablePage
{
    public override IHostScreen HostScreen { get; }

    public ReactiveCommand<Unit, Unit> GoBack { get; set; }
    
    public ReactiveCommand<Unit, Unit> Pay { get; set; }

    public FinalPaymentViewModel(IHostScreen screen) 
    {
        HostScreen = screen;
        
        GoBack = ReactiveCommand.Create(() => // setting value
        {
            screen.GoBack();
        });
        
        Pay = ReactiveCommand.Create(() =>
        {
            screen.GoNext(new TransactionPaymentViewModel(screen));
            
            // FREE TABLE
            OrderSql.transact(screen.CurrentTable.ID);
        });
    }
}