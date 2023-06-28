using System.Reactive;
using ExCSS;
using GUI.ViewModels;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Tables;

namespace GUI.Views;

public class FinalPaymentViewModel : RoutablePage
{
    public TablesService service = new();
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
            service.Free(new Table()
            {
                ID = HostScreen.CurrentTable
            });
        });
    }
}