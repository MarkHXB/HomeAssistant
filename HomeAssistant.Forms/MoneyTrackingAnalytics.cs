using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SkiaSharp;

namespace HomeAssistant.Forms
{
    public partial class MoneyTrackingAnalytics : Form
    {
        public class Goal
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int SavedMoney { get; set; }
            public int SaveMoneyPerMonth { get; set; }

            public override string ToString()
            {
                return $"{Name}";
            }
        }

        public class History
        {
            public string Date { get; set; }
            public string Id { get; set; }

            public override string ToString()
            {
                return $"{Date}";
            }

        }

        private readonly MoneyTrackingForm moneyTrackingForm;
        private IEnumerable<TransactionRecordJson> transactionRecords;
        private IEnumerable<Goal> goalRecords;
        private IEnumerable<History> historyRecords;

        private Goal? SelectedGoal = null;

        private int yearOfTransaction = DateTime.Now.Year;
        private int monthOfTransaction = DateTime.Now.Month;

        private string TransactionFilePath => SelectedTransactionFile ?? (
            string.IsNullOrWhiteSpace(Settings1.Default.LatestUsedTransactionJson) ?
            $"Transaction_{yearOfTransaction}_{monthOfTransaction}.json" :
             Settings1.Default.LatestUsedTransactionJson
           );

        private string? SelectedTransactionFile = null;

        private string GoalsFilePath => $"Goals_{TransactionFilePath}";

        private IEnumerable<TransactionRecordJson> IncomeOfMonthAsc => transactionRecords
            .Where(t => t.Description != "Goal" && t.Field3 > 0)
            .OrderBy(t => t.Date1);

        private IEnumerable<TransactionRecordJson> OutcomefMonthAsc => transactionRecords
            .Where(t => t.Description != "Goal" && t.Field3 < 0)
            .OrderBy(t => t.Date1);

        private IEnumerable<TransactionRecordJson> SavingsForTheSelectedGoal => (transactionRecords
            .Where(t => (t.Description == "Goal" || t.Description == $"Generated automatically for {SelectedGoal?.Name}") && t.Vendor == SelectedGoal?.Name) ?? new List<TransactionRecordJson>())
            .OrderBy(t => t.Field3);


        #region Balance Filters
        private bool IncludeGoalsAsOutcomeAtBalanceChart = false;
        private bool IncludeGoalsAutoAsOutcomeAtBalanceChart = false;

        private IEnumerable<TransactionRecordJson> OutcomeOfMonthFiltered()
        {
            if (IncludeGoalsAsOutcomeAtBalanceChart && IncludeGoalsAutoAsOutcomeAtBalanceChart)
            {
                return transactionRecords.Where(t => t.Field3 < 0);
            }
            else if (IncludeGoalsAsOutcomeAtBalanceChart)
            {
                return transactionRecords.Where(t => t.Field3 < 0 && !t.Description.Contains("automatically"));
            }
            else if (IncludeGoalsAutoAsOutcomeAtBalanceChart)
            {
                return transactionRecords.Where(t => t.Field3 < 0 && t.Description.Contains("automatically"));
            }
            return OutcomefMonthAsc;
        }
        #endregion

        public event EventHandler RefreshEvent;


        public MoneyTrackingAnalytics(MoneyTrackingForm form)
        {
            InitializeComponent();

            transactionRecords = new List<TransactionRecordJson>();
            goalRecords = new List<Goal>();

            moneyTrackingForm = form;

            RefreshEvent += OnRefresh;
        }

        private void OnRefresh(object? sender, EventArgs e)
        {
            transactionRecords = LoadTransactions(TransactionFilePath);

            historyRecords = LoadTransactionHistory();

            goalRecords = GetGoals();

            // Income
            incomeLbl.Text = $"+{IncomeOfMonthAsc.Sum(t => t.Field3)}";

            // Outcome
            outcomeLbl.Text = $"{OutcomefMonthAsc.Sum(t => t.Field3)}";

            // Balance
            // green for the reamining money for this month and red to the outcomes
            int income = IncomeOfMonthAsc.Select(t => t.Field3).Sum();
            int outcome = OutcomeOfMonthFiltered().Select(t => t.Field3).ToList().Select(x => x * -1).Sum();
            int green = income - outcome;
            int red = income - green;
            var balanceSerie = new PieSeries<double> { Values = new double[] { green }, Name = "Balance", Fill = new SolidColorPaint(SKColors.Green) };
            var OutcomeSerie = new PieSeries<double> { Values = new double[] { red }, Name = "Outcomes", Fill = new SolidColorPaint(SKColors.Red) };

            balanceChart.Series = new ISeries[]
                 {
balanceSerie, OutcomeSerie
                 };

            // category chart
            #region Category chart
            if (transactionRecords != null && transactionRecords.Any())
            {
                List<TransactionRecordJson> categorizedTransactions =
                    transactionRecords.Where(t => t.Categories != null && t.Categories.Any()).ToList();

                var groupedData = categorizedTransactions
               .SelectMany(t => t.Categories.Select(c => new { Category = c.Name, t.Field3 }))
               .GroupBy(tc => tc.Category)
               .Select(g => new { Category = g.Key, TotalPrice = g.Sum(tc => tc.Field3) })
               .ToList();

                // Create the SeriesCollection with the grouped data
                var seriesCollection = new ISeries[]
                {
                new ColumnSeries<double>
                {
                    Values = groupedData.Select(gd => ((double)gd.TotalPrice)*-1).ToArray(),
                    Name = "Category Prices"
                }
                };

                // Assign the SeriesCollection to the chart
                cartesianChart1.Series = seriesCollection;

                // Set up the X Axis with category names
                cartesianChart1.XAxes = new Axis[]
                {
                new Axis
                {
                    Name = "Categories",
                    Labels = groupedData.Select(gd => gd.Category).ToArray()
                }
                };

                // Set up the Y Axis
                cartesianChart1.YAxes = new Axis[]
                {
                new Axis
                {
                    Name = "Price",
                    Labeler = value => value.ToString("C") // Format as currency
                }
                };
            }
            #endregion

            // planned savings display the goal price
            if (SelectedGoal != null)
            {
                var saves = SavingsForTheSelectedGoal?.Select(x => x.Field3) ?? new List<int>();
                if (!saves.Any())
                {
                    statusLbl.Text = $"There are no recorded transaction regarding {SelectedGoal.Name} goal saves";
                    return;
                }

                // Define the series
                double x = saves.Select(x => x * -1).Sum();
                double y = SelectedGoal.Price;
                double reamins = (x / y) * 100;
                if (reamins > 0)
                {
                    var pieSeries1 = new PieSeries<double> { Values = new double[] { reamins }, Name = "Saves", Fill = new SolidColorPaint(SKColors.Red) };
                    var pieSeries2 = new PieSeries<double> { Values = new double[] { 100.0 - reamins }, Name = "Reamining", Fill = new SolidColorPaint(SKColors.Wheat) };

                    savingsChart.Series = new ISeries[]
                         {
                        pieSeries1, pieSeries2
                         };
                }
                else
                {
                    var pieSeries1 = new PieSeries<double> { Values = new double[] { 100 }, Name = "Saves", Fill = new SolidColorPaint(SKColors.Green) };

                    savingsChart.Series = new ISeries[]
                         {
                        pieSeries1
                         };
                }

                int reaminingMoney = SelectedGoal.Price - saves.Select(x => x * -1).Sum();
                if (reaminingMoney < 0)
                {
                    savingLbl.Text = $"+{reaminingMoney}";
                    savingLbl.ForeColor = Color.Green;
                }
                else
                {
                    savingLbl.Text = $"-{reaminingMoney}";
                    savingLbl.ForeColor = Color.Orange;
                }

                savingForGoalLbl.Text = $"Savings for {SelectedGoal.Name}";
            }

            // maximum outcome chart

            int allOutcomeSerieValue = OutcomefMonthAsc.Select(t => t.Field3 * -1).Sum();
            int maxOutcomeSerieValue = (Settings1.Default.MaximumOutcomePerMonth - allOutcomeSerieValue) <= 0 ? 0 : Settings1.Default.MaximumOutcomePerMonth - allOutcomeSerieValue;
            var allOutcomeSerie = new PieSeries<double> { Values = new double[] { allOutcomeSerieValue }, Name = "Outcomes", Fill = new SolidColorPaint(SKColors.Red) };
            var maxOutcomeSerie = new PieSeries<double> { Values = new double[] { maxOutcomeSerieValue }, Name = "Maximum", Fill = new SolidColorPaint(SKColors.Green) };

            maxOutcomeChart.Series = new ISeries[]
                 {
                        allOutcomeSerie, maxOutcomeSerie
                 };

            if (maxOutcomeSerieValue == 0)
            {
                maxOutcomeLbl.Text = "Maximum Outcome Reached";
                maxOutcomeLbl.ForeColor = Color.Red;
            }
            else
            {
                maxOutcomeLbl.Text = "Maximum Outcome";
                maxOutcomeLbl.ForeColor = Color.Black;
            }

            // history
            historyListBox.Items.Clear();
            foreach (History history in historyRecords)
            {
                historyListBox.Items.Add(history);
            }

            // goals
            goalsList.Items.Clear();
            foreach (Goal goal in goalRecords)
            {
                goalsList.Items.Add(goal);
            }
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

        private IEnumerable<History> LoadTransactionHistory()
        {
            IList<TransactionRecordJson> transactionRecords = new List<TransactionRecordJson>();
            List<string> filesWithPrefix = new List<string>();
            List<History> history = new List<History>();
            string prefix = "Transaction_";

            // Get all files in the current directory
            string[] allFiles = Directory.GetFiles("./");

            // Iterate through each file and check if it starts with the given prefix
            foreach (string file in allFiles)
            {
                // Get the file name without the path
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith(prefix))
                {
                    filesWithPrefix.Add(file);
                }
            }

            foreach (string file in filesWithPrefix)
            {
                string content = File.ReadAllText(file);
                IEnumerable<TransactionRecordJson> transactionRecordsJson = JsonConvert.DeserializeObject<List<TransactionRecordJson>>(content) ?? new List<TransactionRecordJson>();
                string id = Path.GetFileNameWithoutExtension(file);
                string dateRaw = string.Empty;
                foreach (TransactionRecordJson transaction in transactionRecordsJson.OrderBy(x => x.Date1))
                {
                    if (!string.IsNullOrEmpty(transaction.Date1))
                    {
                        dateRaw = transaction.Date1;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(dateRaw))
                {
                    continue;
                }

                history.Add(new History() { Date = dateRaw.Substring(0, dateRaw.Length - 2), Id = id });
            }

            return history;
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
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void goalsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (goalsList.SelectedIndex == -1)
            {
                return;
            }

            int index = goalsList.SelectedIndex;
            Goal goal = (Goal)goalsList.Items[index];

            if (goal == null)
            {
                return;
            }

            SelectedGoal = goal;

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historyListBox.SelectedIndex == -1)
            {
                return;
            }

            int index = historyListBox.SelectedIndex;
            History history1 = (History)historyListBox.Items[index];

            if (history1 == null)
            {
                return;
            }

            List<string> filesWithPrefix = new List<string>();
            List<History> history = new List<History>();
            string prefix = "Transaction_";

            // Get all files in the current directory
            string[] allFiles = Directory.GetFiles("./");

            // Iterate through each file and check if it starts with the given prefix
            foreach (string file in allFiles)
            {
                // Get the file name without the path
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.StartsWith(prefix))
                {
                    filesWithPrefix.Add(fileName);
                }
            }

            string path = filesWithPrefix.FirstOrDefault(x =>
            {
                return x.EndsWith(history1.Id);
            }) ?? string.Empty;

            SelectedTransactionFile = path + ".json";

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void addGoalBtn_Click(object sender, EventArgs e)
        {
            string goalName = Interaction.InputBox("Name of your goal:", "Insert Name");
            string goalPrice = Interaction.InputBox("Money to achieve the goal in Forint (FT):", "Insert Price");

            if (string.IsNullOrWhiteSpace(goalName))
            {
                statusLbl.Text = "You should give a valid goal name";
                return;
            }
            if (string.IsNullOrWhiteSpace(goalPrice))
            {
                statusLbl.Text = "You should give a valid goal price";
                return;
            }

            int.TryParse(goalPrice, out int goalPriceResult);

            if (goalPriceResult == 0)
            {
                statusLbl.Text = "You give incorrect goal price";
                return;
            }

            SetGoals(new Goal() { Name = goalName, Price = goalPriceResult, SavedMoney = 0 });

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerable<Goal> GetGoals()
        {
            var goals = new List<Goal>();

            if (File.Exists(GoalsFilePath))
            {
                string content = File.ReadAllText(GoalsFilePath);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    goals = JsonConvert.DeserializeObject<List<Goal>>(content) ?? new List<Goal>();
                }
            }
            else
            {
                File.Create(GoalsFilePath).Close();
            }

            return goals;
        }

        private void SetGoals(Goal? specificGoal = null, bool savingModified = false)
        {
            if (!File.Exists(GoalsFilePath))
            {
                File.Create(GoalsFilePath).Close();
            }

            string content = File.ReadAllText(GoalsFilePath);

            var goals = JsonConvert.DeserializeObject<List<Goal>>(content) ?? new List<Goal>();

            if (specificGoal != null)
            {
                if (savingModified)
                {
                    var _goal = goals.FirstOrDefault(x => x.Name == specificGoal.Name);

                    if (_goal == null)
                    {
                        statusLbl.Text = "Not found that specific goal to save money for";
                        return;
                    }

                    _goal.SavedMoney = specificGoal.SavedMoney;
                }
                else
                {
                    if (!goals.Any(c => c.Name == specificGoal.Name))
                    {
                        goals.Add(specificGoal);
                    }
                }
            }
            else
            {
                foreach (Goal goal in goalsList.Items)
                {
                    if (!goals.Any(c => c.Name == goal.Name))
                    {
                        goals.Add(goal);
                    }
                }
            }

            string json = JsonConvert.SerializeObject(goals, Formatting.Indented);
            File.WriteAllText(GoalsFilePath, json);
        }

        private void addSavingToCurrentMonthBtn_Click(object sender, EventArgs e)
        {
            if (SelectedGoal == null)
            {
                MessageBox.Show("First you should select a goal!");
                return;
            }

            string goalSaving = Interaction.InputBox("Saving in current month in Forint (FT):", "Insert Price");

            if (string.IsNullOrWhiteSpace(goalSaving))
            {
                statusLbl.Text = "You should give a valid goal saving price";
                return;
            }

            int.TryParse(goalSaving, out int goalSavingResult);

            if (goalSavingResult == 0)
            {
                statusLbl.Text = "You give incorrect goal saving price";
                return;
            }

            SelectedGoal.SavedMoney += goalSavingResult;

            // Set goals
            SetGoals(SelectedGoal, true);

            // Insert transaction
            InsertTransaction(new TransactionRecordJson()
            {
                Vendor = SelectedGoal.Name,
                Field3 = goalSavingResult * -1,
                Date1 = DateTime.Now.ToShortDateString(),
                Date2 = DateTime.Now.ToShortDateString(),
                Description = "Goal"
            });
        }

        private void InsertTransaction(TransactionRecordJson transactionRecordJson)
        {
            string path = TransactionFilePath;

            if (!File.Exists(path))
            {
                MessageBox.Show("Transaction file not found");
                return;
            }

            string content = File.ReadAllText(path);

            var transactionsOld = JsonConvert.DeserializeObject<List<TransactionRecordJson>>(content) ?? new List<TransactionRecordJson>();

            transactionsOld.Add(transactionRecordJson);

            string json = JsonConvert.SerializeObject(transactionsOld, Formatting.Indented);
            File.WriteAllText(path, json);

            MessageBox.Show($"Successfully inserted a new transaction to {path}");

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool HasAutomaticSavingDoneForGoal(string goalName)
        {
            string id = $"Generated automatically for {goalName}";
            return transactionRecords.Any(t => t.Description == id);
        }

        private void addAutomaticSavingPerMBtn_Click(object sender, EventArgs e)
        {
            if (SelectedGoal == null)
            {
                MessageBox.Show("First you should select a goal!");
                return;
            }
            if (transactionRecords == null)
            {
                MessageBox.Show("There are no transactions for this month, maybe you should import it first!");
                return;
            }

            if (HasAutomaticSavingDoneForGoal(SelectedGoal.Name))
            {
                DialogResult result = MessageBox.Show("Do you want to update {} automatic saving per month price?",
                                                   "Confirm Update Price",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            string savePrice = Interaction.InputBox($"Update the save money per month for {SelectedGoal.Name}:", "Insert Price");

            if (string.IsNullOrWhiteSpace(savePrice))
            {
                statusLbl.Text = "You should give a valid goal saving price";
                return;
            }

            int.TryParse(savePrice, out int goalSavingResult);

            if (goalSavingResult == 0)
            {
                statusLbl.Text = "You give incorrect goal saving price";
                return;
            }

            SelectedGoal.SaveMoneyPerMonth = goalSavingResult;

            // Set goals
            SetGoals(SelectedGoal, true);

            // Insert transaction
            InsertTransaction(new TransactionRecordJson()
            {
                Vendor = SelectedGoal.Name,
                Field3 = goalSavingResult * -1,
                Date1 = DateTime.Now.ToShortDateString(),
                Date2 = DateTime.Now.ToShortDateString(),
                Description = $"Generated automatically for {SelectedGoal.Name}"
            });
        }

        private void balanceGoalsFilterChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (balanceGoalsFilterChkBox.Checked)
            {
                IncludeGoalsAsOutcomeAtBalanceChart = true;
            }
            else
            {
                IncludeGoalsAsOutcomeAtBalanceChart = false;
            }

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void balanceGoalsAutoFilterChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (balanceGoalsAutoFilterChkBox.Checked)
            {
                IncludeGoalsAutoAsOutcomeAtBalanceChart = true;
            }
            else
            {
                IncludeGoalsAutoAsOutcomeAtBalanceChart = false;
            }

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void modifyMaxOutcomeBtn_Click(object sender, EventArgs e)
        {
            string maxOutcomePrice = Interaction.InputBox($"Update maximum outcome value per month:", "Modify Price");

            if (string.IsNullOrWhiteSpace(maxOutcomePrice))
            {
                statusLbl.Text = "You should give a valid max outcome price";
                return;
            }

            int.TryParse(maxOutcomePrice, out int maxOutcomePriceResult);

            if (maxOutcomePriceResult == 0)
            {
                statusLbl.Text = "You give incorrect max outcome price";
                return;
            }

            Settings1.Default.MaximumOutcomePerMonth = maxOutcomePriceResult;
            Settings1.Default.Save();

            MessageBox.Show("Successfully modified maximum outcome value");

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
