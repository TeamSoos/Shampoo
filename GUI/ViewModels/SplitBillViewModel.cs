using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using ReactiveUI;
using GUI.ViewModels;
using GUI.Views;
using ModelLayer.Tables;
using ServiceLayer.Payment;

namespace GUI.ViewModels
{
    public class SplitBillViewModel : RoutablePage
    {
        public override IHostScreen HostScreen { get; }
        
        private PaymentService service = new ();
        public ReactiveCommand<Unit, Unit> GoBack { get; set; }
        public ReactiveCommand<Unit, Unit> ConfirmSplitBillButton { get; set; }
        public ReactiveCommand<Unit, Unit> ConfirmButton { get; set; }
        public ReactiveCommand<Unit, Unit> CalculateButton { get; set; }
        
        public string customerTotal;
        public string CustomerTotal
        {
            get { return customerTotal;}
            set => this.RaiseAndSetIfChanged(ref customerTotal, value);
        }

        public string _total;
        
        public string Total
        {
            get { return $"Total: €{_total}";}
            set
            {
                this.RaiseAndSetIfChanged(ref _total, value);
            }
        }

        private int splitCount = 1;
        public int SplitCount
        {
            get => splitCount;
            set
            {
                this.RaiseAndSetIfChanged(ref splitCount, value);
            }
        }

        public string eachPersonPays;
        public string EachPersonPays
        {
            get => eachPersonPays;
            private set => this.RaiseAndSetIfChanged(ref eachPersonPays, value);
        }

        public ReactiveCommand<Unit, Unit> IncrementButton { get; }
        public ReactiveCommand<Unit, Unit> DecrementButton { get; }

        public SplitBillViewModel(IHostScreen screen)
        {
            HostScreen = screen;
            
            IncrementButton = ReactiveCommand.Create(() =>
            {
                SplitCount++;
            });
            
            DecrementButton = ReactiveCommand.Create(() =>
            {
                if (SplitCount > 1)
                    SplitCount--;
            });
            
            GoBack = ReactiveCommand.Create(() =>
            {
                screen.GoBack();
            });
            
            CalculateButton = ReactiveCommand.Create(() =>
            {
                CalculateEachPersonPays();
            });
            
            ConfirmSplitBillButton = ReactiveCommand.Create(() =>
            {
                string total = _total.Replace("Total: €", ""); // get only the numeric part
                string eachPays = eachPersonPays.Replace("€", ""); // get only the numeric part
                screen.GoNext(new FinalPaymentViewModel(screen, total, eachPays));
            });
            
            ConfirmButton = ReactiveCommand.Create(() =>
            {
                screen.GoNext(new FinalPaymentViewModel(screen, CustomerTotal, EachPersonPays));
            });
            
            int table = screen.CurrentTable.ID;
            var res = service.GetTotalPrice(new Table()
            {
                ID = table
            });
            
            Total = res.TotalAmount.ToString("00.00");
        }

        public void CalculateEachPersonPays()
        {
            decimal totalPrice;

            // If the user has entered a custom total, use it. Otherwise, use the total from the bill.
            if (decimal.TryParse(CustomerTotal, out decimal customerTotal))
            {
                totalPrice = customerTotal;
            }
            else
            {
                totalPrice = decimal.Parse(_total.Replace("Total: €", ""));
            }
    
            EachPersonPays = (totalPrice / SplitCount).ToString("0.##€");
        }

    }
}
