using Newtonsoft.Json;

namespace HomeAssistant.Forms
{
    public partial class MoneyTrackingAnalytics : Form
    {
        private readonly MoneyTrackingForm moneyTrackingForm;
        private IEnumerable<TransactionRecordJson> transactionRecords;

        private int yearOfTransaction = DateTime.Now.Year;
        private int monthOfTransaction = DateTime.Now.Month;
        private string TransactionFilePath => $"Transaction_{yearOfTransaction}_{monthOfTransaction}.json";

        private IEnumerable<TransactionRecordJson> IncomeOfMonthAsc => transactionRecords
            .Where(t => t.Field3 > 0)
            .OrderBy(t => t.Date1);

        private IEnumerable<TransactionRecordJson> OutcomefMonthAsc => transactionRecords
            .Where(t => t.Field3 < 0)
            .OrderBy(t => t.Date1);

        public MoneyTrackingAnalytics(MoneyTrackingForm form)
        {
            InitializeComponent();

            transactionRecords = new List<TransactionRecordJson>();

            moneyTrackingForm = form;
        }

        private IEnumerable<TransactionRecordJson> LoadTransactions(string path)
        {
            IList<TransactionRecordJson> transactionRecords = new List<TransactionRecordJson>();

            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                var transactions = JsonConvert.DeserializeObject<IList<TransactionRecordJson>>(content);

                if (transactions != null && transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        transactionRecords.Add(transaction);
                    }
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                statusLbl.Text = "Transaction file not found";
            }

            return transactionRecords;
        }

        private void backBtn_Click(object sender, EventArgs e)
        {

            Hide();
            moneyTrackingForm.Show();
        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        private void MoneyTrackingAnalytics_Load(object sender, EventArgs e)
        {
            transactionRecords = LoadTransactions(TransactionFilePath);

            // Income
            int[] dataX = IncomeOfMonthAsc.Select(t => t.Field3).ToArray();
            int[] dataY = IncomeOfMonthAsc.Select(t => t.Field3).ToArray();
            formsPlot1.Plot.Add.Scatter(dataX, dataY);
            formsPlot1.Refresh();
            incomeLbl.Text = $"+{IncomeOfMonthAsc.Sum(t => t.Field3)}";

            // Outcome
            dataX = OutcomefMonthAsc.Select(t => t.Field3).ToArray();
            dataY = OutcomefMonthAsc.Select(t => t.Field3).ToArray();
            formsPlot3.Plot.Add.Scatter(dataX, dataY);
            formsPlot3.Refresh();
            outcomeLbl.Text = $"-{OutcomefMonthAsc.Sum(t => t.Field3)}";
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
