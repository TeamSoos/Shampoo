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

        private string _total;
        
        public string Total
        {
            get { return $"Total: €{_total}";}
            set
            {
                this.RaiseAndSetIfChanged(ref _total, value);
            }
        }

        private int splitCount = 0;
        public int SplitCount
        {
            get => splitCount;
            set
            {
                this.RaiseAndSetIfChanged(ref splitCount, value);
            }
        }

        private string eachPersonPays;
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
                screen.GoNext(new FinalPaymentViewModel(screen, Total, EachPersonPays ));
            });
            
            ConfirmSplitBillButton = ReactiveCommand.Create(() =>
            {
                screen.GoNext(new FinalPaymentViewModel(screen));
            });
            
            ConfirmButton = ReactiveCommand.Create(() =>
            {
                screen.GoNext(new FinalPaymentViewModel(screen));
            });
            
            int table = screen.CurrentTable.ID;
            var res = service.GetTotalPrice(new Table()
            {
                ID = table
            });
            
            Total = res.TotalAmount.ToString("00.00");
        }

        private void CalculateEachPersonPays()
        {
            decimal totalPrice = decimal.Parse(Total.Replace("Total: €", "0.00"));
            EachPersonPays = (totalPrice / SplitCount).ToString("0.##€");
        }
    }
}
