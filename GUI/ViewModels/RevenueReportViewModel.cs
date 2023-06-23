using System.Collections.ObjectModel;
using System.Reactive;
using ExCSS;
using GUI.Views;
using Logic.SQL;
using Npgsql;
using ReactiveUI;

namespace GUI.ViewModels;

public class RevenueReportViewModel : RoutablePage{
    public RevenueReportViewModel(IHostScreen screen) {
        HostScreen = screen;

        BackButton = ReactiveCommand.Create(() =>
        {
            HostScreen.GoBack();
        });

        
        List<TransactionItem> items = new List<TransactionItem>();

        Task.Run(
            async () =>
            {
                items = await GetAll();
            }
        ).Wait();

        Transactions = new TransactionListViewModel(
            items
        );

        foreach (var item in Transactions.Items) {
            MonthlyTotal += item.total;
        }
    }

    public ReactiveCommand<Unit, Unit> BackButton { get; set; }

    public override IHostScreen HostScreen { get; }
  
    public TransactionListViewModel Transactions { get; }
    public double MonthlyTotal { get; }

    public class TransactionItem {
        public int id { get; set; }
        public int table_id { get; set; }
        public int employee_id { get; set; }
        public string payment_type { get; set; }
        public double total { get; set; }
        public DateTime time { get; set; }
    }
  

    public class TransactionListViewModel : ViewModelBase {
        public TransactionListViewModel(IEnumerable<TransactionItem> items) {
            Items = new ObservableCollection<TransactionItem>(items);
        }

        public ObservableCollection<TransactionItem> Items { get; set; }
    }
  
    public async static Task<List<TransactionItem>> GetAll()
    {
        Library.Database db = new Library.Database();

        var cmd = new NpgsqlCommand("SELECT id, time, employee_id, total, payment_type, table_id FROM transactions ORDER BY time DESC", db.Conn);

        

        var reader = await db.Query(cmd);
        List<TransactionItem> items = new List<TransactionItem>();

        while (reader.Read())
        {
            items.Add(new TransactionItem() {
                employee_id = (int)reader["employee_id"],
                table_id = (int)reader["table_id"],
                id = (int)reader["id"],
                total = (double)reader["total"],
                time = (DateTime)reader["time"],
                payment_type = (string)reader["payment_type"],
            });
        }

        return items;
    }
}