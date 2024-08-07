using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace HomeAssistant.Forms
{
    public partial class MoneyTrackingForm : Form
    {
        private const string TransactionUrl = "https://www.otpbankdirekt.hu/homebank/do/hb2/menuaccess?hb2NavmenuSelection=SZAMLATORTENET";
        private const string CategoryFilePath = "Categories.json";
        private int yearOfTransaction = DateTime.Now.Year;
        private int monthOfTransaction = DateTime.Now.Month;
        private string TransactionFilePath => string.IsNullOrWhiteSpace(Settings1.Default.LatestUsedTransactionJson) ?
            $" Transaction_{yearOfTransaction}_{monthOfTransaction}.json" :
            Settings1.Default.LatestUsedTransactionJson;
        private bool userWantsToReplaceTransactionFile = false;

        private bool isActiveForm = false;

        private List<TransactionRecordJson> transactionRecords;

        /// <summary>
        /// Foreground and background
        /// </summary>
        private (Color, Color) CategorizedTransactionStateColoring = (Color.Black, Color.Green);

        /// <summary>
        /// Foreground and background
        /// </summary>
        private (Color, Color) UnCategorizedTransactionStateColoring = (Color.Black, Color.Yellow);

        public event EventHandler RefreshEvent;

        private bool originalTransactionsSelected = true;

        public MoneyTrackingForm()
        {
            InitializeComponent();

            transactionRecords = new List<TransactionRecordJson>();

            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            foreach (Category category in GetCategories())
            {
                categoryList.Items.Add(category);
            }

            RefreshEvent += OnRefreshForm;

            isActiveForm = true;

            LoadTransactions(TransactionFilePath);
        }

        private void OnRefreshForm(object? sender, EventArgs e)
        {
            if (ShouldUpdateImportedTransactionsLabel() || string.IsNullOrWhiteSpace(TransactionFilePath))
            {
                shouldUpdateExportLbl.Text = "You should do the steps before any action!";
                analyzeBtn.Enabled = false;
            }
            else
            {
                shouldUpdateExportLbl.Text = "";
                analyzeBtn.Enabled = true;
            }
        }

        private bool ShouldUpdateImportedTransactionsLabel()
        {
            if (transactionList.Items.Count == 0)
            {
                return true;
            }

            List<TransactionRecordJson> transactions = new List<TransactionRecordJson>();

            foreach (TransactionRecordJson item in transactionList.Items)
            {
                transactions.Add(item);
            }

            int currentMonth = DateTime.Now.Month;
            var first = transactions.OrderByDescending(t => t.Date1).First();
            if (first == null)
            {
                statusLbl.Text = "Selected transaction is null";
                return true;
            }
            string transactionMonth = first.Date1.Substring(4, 2);
            int.TryParse(transactionMonth, out int transactionMonthResult);

            if (transactionMonthResult == 0)
            {
                return false;
            }

            return (currentMonth - 1 == 0 ? 1 : currentMonth - 1) > transactionMonthResult;
        }

        private void LoadTransactions(string path, string? searchQuery = null)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                var transactions = JsonConvert.DeserializeObject<IList<TransactionRecordJson>>(content);

                if (transactions != null && transactions.Any())
                {
                    transactionList.Items.Clear();
                    transactionRecords.Clear();

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        transactions = transactions
                            .Where(t => t.Vendor.ToLower().Contains(searchQuery.ToLower()))
                            .ToList();
                    }

                    transactions = transactions.OrderBy(t => t.Field3).ToList();

                    if (!originalTransactionsSelected)
                    {
                        transactions = transactions.Where(t => t?.Categories.Any() ?? false).ToList();
                    }

                    foreach (var transaction in transactions)
                    {
                        transactionList.Items.Add(transaction);
                        transactionRecords.Add(transaction);
                    }
                }
            }

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ReplaceTransactionFile()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to replace the file?",
                                                   "Confirm File Replacement",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Code to replace the file
                userWantsToReplaceTransactionFile = true;
            }
            else
            {
                // Code to handle the case when user does not want to replace the file
                MessageBox.Show("File replacement has been cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            OpenUrl(TransactionUrl);
        }

        #region Private implementation
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
        #endregion

        // Function to get user input for year and month
        public (string Year, string Month) GetYearAndMonth()
        {
            string year = Interaction.InputBox("Year of:", "Rename", DateTime.Now.Year.ToString());
            string month = Interaction.InputBox("Month of:", "Rename", DateTime.Now.Month.ToString());

            return (year, month);
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                string content = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(content))
                {
                    statusLbl.Text = "The exported CSV file is empty.";
                    return;
                }

                Settings1.Default.TransactionFilePath = path;
                Settings1.Default.Save();

                var (year, month) = GetYearAndMonth();
                int.TryParse(year, out int yearInt);
                int.TryParse(month, out int monthInt);

                if (yearInt < 1)
                {
                    statusLbl.Text = "The given transaction year is invalid. Try export the transactions with a valid a year.";
                    return;
                }
                if (monthInt < 1)
                {
                    statusLbl.Text = "The given transaction month is invalid. Try export the transactions with a valid a month.";
                    return;
                }
                yearOfTransaction = yearInt;
                monthOfTransaction = monthInt;

                Settings1.Default.LatestUsedTransactionJson = TransactionFilePath;
                Settings1.Default.Save();

                if (File.Exists(TransactionFilePath))
                {
                    ReplaceTransactionFile();
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = false,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    TrimOptions = TrimOptions.Trim,
                    Quote = '"',
                };

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<TransactionRecord>().ToList();

                    transactionList.Items.Clear();

                    foreach (var record in records)
                    {
                        // Process each record
                        // Create a new TransactionRecordJson instance and generate a unique ID
                        var transactionRecordJson = new TransactionRecordJson
                        {
                            Id = Guid.NewGuid().ToString(),
                            Field1 = record.Field1,
                            Field2 = record.Field2,
                            Field3 = record.Field3,
                            Currency = record.Currency,
                            Date1 = record.Date1,
                            Date2 = record.Date2,
                            Field7 = record.Field7,
                            Field8 = record.Field8,
                            Vendor = record.Vendor,
                            Description = record.Description,
                            Field11 = record.Field11,
                            Field12 = record.Field12,
                            Field13 = record.Field13,
                            TransactionType = record.TransactionType,
                            Field15 = record.Field15,
                            Categories = GetCategoriesBasedOnTransactionVendor(record.Vendor) ?? new List<Category>()
                        };

                        // Process each record
                        transactionList.Items.Add(transactionRecordJson);
                        transactionRecords.Add(transactionRecordJson);
                    }
                }

                if (ShouldUpdateImportedTransactionsLabel())
                {
                    shouldUpdateExportLbl.Text = "You should do the steps before any action!";
                }
                else
                {
                    shouldUpdateExportLbl.Text = "";
                }

                analyzeBtn.Enabled = transactionList.Items.Count != 0;
            }
            else
            {

            }

            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        private List<Category>? GetCategoriesBasedOnTransactionVendor(string vendor)
        {
            List<List<TransactionRecordJson>> history = MoneyTrackingUtilities.GetHistory();

            foreach (List<TransactionRecordJson> transactions in history)
            {
                foreach (var transaction in transactions)
                {
                    if (transaction.Vendor == vendor)
                    {
                        return transaction.Categories;
                    }
                }
            }

            return null;
        }

        private void categoryList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();

            if (File.Exists(CategoryFilePath))
            {
                string content = File.ReadAllText(CategoryFilePath);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    categories = JsonConvert.DeserializeObject<List<Category>>(content) ?? new List<Category>();
                }
            }
            else
            {
                File.Create(CategoryFilePath).Close();
            }

            return categories;
        }

        private void SetCategories()
        {
            if (!File.Exists(CategoryFilePath))
            {
                File.Create(CategoryFilePath).Close();
            }

            string content = File.ReadAllText(CategoryFilePath);

            var categories = JsonConvert.DeserializeObject<List<Category>>(content) ?? new List<Category>();

            foreach (Category category in categoryList.Items)
            {
                if (!categories.Any(c => c.Name == category.Name))
                {
                    categories.Add(category);
                }
            }

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText(CategoryFilePath, json);
        }

        private void SetTransaction()
        {
            if (!File.Exists(TransactionFilePath))
            {
                File.Create(TransactionFilePath).Close();
                var transactions = JsonConvert.SerializeObject(transactionRecords, Formatting.Indented);
                File.WriteAllText(TransactionFilePath, transactions);
                return;
            }
            if (userWantsToReplaceTransactionFile)
            {
                File.WriteAllText(TransactionFilePath, string.Empty);
                var transactions = JsonConvert.SerializeObject(transactionRecords, Formatting.Indented);
                File.WriteAllText(TransactionFilePath, transactions);
                MessageBox.Show("File has been replaced.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                userWantsToReplaceTransactionFile = false;
                return;
            }

            string content = File.ReadAllText(TransactionFilePath);

            var transactionsOld = JsonConvert.DeserializeObject<List<TransactionRecordJson>>(content) ?? new List<TransactionRecordJson>();

            for (int i = 0; i < transactionsOld.Count; i++)
            {
                foreach (TransactionRecordJson transactionNew in transactionRecords)
                {
                    if (transactionsOld[i].Id == transactionNew.Id)
                    {
                        transactionsOld[i] = transactionNew;
                    }
                }
            }

            string json = JsonConvert.SerializeObject(transactionsOld, Formatting.Indented);
            File.WriteAllText(TransactionFilePath, json);
        }

        private void MoneyTrackingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // save categories
            SetCategories();

            // save transaction
            SetTransaction();
        }

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            categoryList.Items.Add(new Category());
        }

        private void categoryList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = categoryList.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    // Select the item under the mouse pointer
                    categoryList.SelectedIndex = index;

                    // Show the context menu
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = categoryList.SelectedIndex;
            if (index != -1)
            {
                Category? currentItem = categoryList.Items[index] as Category;

                string newName = Microsoft.VisualBasic.Interaction.InputBox("Rename item:", "Rename");

                if (!string.IsNullOrEmpty(newName) && newName != currentItem.Name)
                {
                    (categoryList.Items[index] as Category).Name = newName;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = categoryList.SelectedIndex;
            if (index != -1)
            {
                categoryList.Items.RemoveAt(index);
            }
        }

        private void transactionList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            // Get the item text
            TransactionRecordJson transaction = transactionList.Items[e.Index] as TransactionRecordJson;

            // Draw the background
            e.DrawBackground();

            if (transaction.Categories.Any())
            {
                // Draw the item text
                using (SolidBrush brush = new SolidBrush(CategorizedTransactionStateColoring.Item1))
                {
                    e.Graphics.DrawString(transaction.ToString(), e.Font, brush, e.Bounds);
                }
            }
            else
            {
                // Draw the item text
                using (SolidBrush brush = new SolidBrush(UnCategorizedTransactionStateColoring.Item1))
                {
                    e.Graphics.DrawString(transaction.ToString(), e.Font, brush, e.Bounds);
                }
            }

            // Draw the focus rectangle if the mouse hovers over an item
            e.DrawFocusRectangle();
        }

        private void transactionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (transactionList.SelectedIndex == -1)
            {
                return;
            }

            var selectedTransaction = transactionList.Items[transactionList.SelectedIndex] as TransactionRecordJson;

            if (selectedTransaction?.Categories != null && selectedTransaction.Categories.Any())
            {
                foreach (var category in selectedTransaction?.Categories)
                {
                    for (int i = 0; i < categoryList.Items.Count; i++)
                    {
                        Category c = categoryList.Items[i] as Category;
                        if (c?.Name == category.Name)
                        {
                            categoryList.SetItemChecked(i, true);
                        }
                        else
                        {
                            categoryList.SetItemChecked(i, false);
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < categoryList.Items.Count; i++)
                {
                    categoryList.SetItemChecked(i, false);
                }
            }
        }

        private void analyzeBtn_Click(object sender, EventArgs e)
        {
            using (MoneyTrackingAnalytics form = new MoneyTrackingAnalytics(this))
            {
                isActiveForm = false;
                Hide();
                form.ShowDialog();
            }

            isActiveForm = true;

            LoadTransactions(TransactionFilePath);
        }

        private void MoneyTrackingForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!isActiveForm)
            {
                // save categories
                SetCategories();

                // save transaction
                SetTransaction();
            }
        }

        private void searchTxtBox_TextChanged(object sender, EventArgs e)
        {
            string query = searchTxtBox.Text;

            transactionList.Items.Clear();

            if (string.IsNullOrWhiteSpace(query))
            {
                transactionRecords.ForEach(t => transactionList.Items.Add(t));
                return;
            }

            var referenceList = new TransactionRecordJson[transactionRecords.Count];
            var result = new List<TransactionRecordJson>();

            transactionRecords.CopyTo(referenceList, 0);

            result = referenceList
                                  .Where(t => t.Vendor.ToLower().Contains(query.ToLower()))
                                  .ToList();

            result.ForEach(t => transactionList.Items.Add(t));
        }

        private void assignCategoriesBtn_Click(object sender, EventArgs e)
        {
            var categories = new List<Category>();

            foreach (Category category in categoryList.CheckedItems)
            {
                categories.Add(new Category() { IsChecked = true, Name = category.Name });
            }
            foreach (TransactionRecordJson transaction in transactionList.CheckedItems)
            {
                transaction.Categories = categories;
            }

            SetTransaction();

            LoadTransactions(TransactionFilePath);
        }

        private void originalTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            originalTransactionsSelected = true;
            LoadTransactions(TransactionFilePath);

            if (sender is ToolStripMenuItem clickedItem)
            {
                clickedItem.Font = new Font(clickedItem.Font, FontStyle.Bold);
                categorizedTransactionsToolStripMenuItem.Font = new Font(clickedItem.Font, FontStyle.Regular);
            }
        }

        private void categorizedTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            originalTransactionsSelected = false;
            LoadTransactions(TransactionFilePath);

            if (sender is ToolStripMenuItem clickedItem)
            {
                clickedItem.Font = new Font(clickedItem.Font, FontStyle.Bold);
                originalTransactionsToolStripMenuItem.Font = new Font(clickedItem.Font, FontStyle.Regular);
            }
        }
    }
}
