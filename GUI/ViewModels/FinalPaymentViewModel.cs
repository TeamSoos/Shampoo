using System.Reactive;
using ExCSS;
using GUI.Logic.Models.Order;
using GUI.ViewModels;
using ModelLayer.Tables;
using ReactiveUI;
using ServiceLayer.Tables;

namespace GUI.ViewModels;

public class FinalPaymentViewModel : RoutablePage
{
    public TablesService service = new();
    public override IHostScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> GoBack { get; set; }
    public ReactiveCommand<Unit, Unit> Pay { get; set; }
    
    private string total;
    public string Total
    {
        get => total;
        private set => this.RaiseAndSetIfChanged(ref total, value);
    }

    private string pricePerPerson;
    public string PricePerPerson
    {
        get => pricePerPerson;
        private set => this.RaiseAndSetIfChanged(ref pricePerPerson, value);
    }

    
    public FinalPaymentViewModel(IHostScreen screen)
    {
        HostScreen = screen;

        GoBack = ReactiveCommand.Create(() => 
        {
            screen.GoBack();
        });

        Pay = ReactiveCommand.Create(() =>
        {
            screen.GoNext(new TransactionPaymentViewModel(screen));

            // free table
            service.Free(new Table()
            {
                ID = HostScreen.CurrentTable.ID
            });
        });
    }
    
    public FinalPaymentViewModel(IHostScreen screen, string totalPrice, string pricePerPerson) : this(screen)
    {
        Total = totalPrice;
        PricePerPerson = pricePerPerson;
    }
}