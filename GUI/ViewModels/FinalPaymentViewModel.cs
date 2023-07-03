using System.Reactive;
using ExCSS;
using GUI.Logic.Models.Order;
using GUI.ViewModels;
using ModelLayer.Payment;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class FinalPaymentViewModel : RoutablePage
{
    public TablesService service = new();
    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }
    public ReactiveCommand<string, Unit> Pay { get; set; }
    
    private string total;
    public string Total
    {
        get { return total;}
        set => this.RaiseAndSetIfChanged(ref total, value);
    }

    private string pricePerPerson;
    public string PricePerPerson
    {
        get { return pricePerPerson;}
        set => this.RaiseAndSetIfChanged(ref pricePerPerson, value);
    }
    
    public string customerComment;
    public string CustomerComment
    {
        get { return $"Customer Comment: {customerComment}";}
        private set => this.RaiseAndSetIfChanged(ref customerComment, value);
    }
    
    public FinalPaymentViewModel(IHostScreen screen)
    {
        HostScreen = screen;

        GoBack = ReactiveCommand.Create(() => 
        {
            screen.GoBack();
        });

        Pay = ReactiveCommand.Create<string>((paymentType) =>
        {
            screen.GoNext(new TransactionPaymentViewModel(screen, 
                    new PaymentModel()
                    {
                        TotalAmount = int.Parse(total ?? ""),
                        EmployeeId = screen.CurrentUser.ID,
                        TableId = screen.CurrentTable.ID,
                        PaymentType = paymentType,
                    }
                ));

            // free table
            service.Free(new Table()
            {
                ID = HostScreen.CurrentTable.ID
            });
        });
    }
    
    public FinalPaymentViewModel(IHostScreen screen, string totalPrice, string pricePerPerson) : this(screen)
    {
        Total = $"Total: €{totalPrice}";
        PricePerPerson = $"Price per Person: €{pricePerPerson}";
    }
    
    public FinalPaymentViewModel(IHostScreen screen, string totalPrice, string pricePerPerson, string customerComment) : this(screen, totalPrice, pricePerPerson)
    {
        CustomerComment = customerComment;
    }
}