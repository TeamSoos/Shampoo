using System.Reactive;
using GUI.ViewModels;
using ModelLayer.Payment;
using ReactiveUI;
using ServiceLayer.Payment;

namespace GUI.ViewModels;

public class TransactionPaymentViewModel : RoutablePage
{
    public override IHostScreen HostScreen { get; }
    PaymentService service = new PaymentService();
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }

    public TransactionPaymentViewModel(IHostScreen screen, PaymentModel payment)
    {
        HostScreen = screen;
        GoBack = ReactiveCommand.Create(() => 
        {
            screen.GoBack();
        });

        

        service.Create(payment);
    }
}