using System.Reactive;
using GUI.ViewModels;
using ReactiveUI;

namespace GUI.ViewModels;

public class TransactionPaymentViewModel : RoutablePage
{
    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }


    public TransactionPaymentViewModel(IHostScreen screen)
    {
        HostScreen = screen;
        GoBack = ReactiveCommand.Create(() => 
        {
            screen.GoBack();
        });
    }
}