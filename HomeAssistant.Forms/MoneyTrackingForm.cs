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
        private string TransactionFilePath => $"Transaction_{yearOfTransaction}_{monthOfTransaction}.json";
        private bool userWantsToReplaceTransactionFile = false;

        private int lastSelectedTransactionIndex = -1;

        private bool isActiveForm = false;

        /// <summary>
        /// Foreground and background
        /// </summary>
        private (Color, Color) CategorizedTransactionStateColoring = (Color.Black, Color.Green);

        /// <summary>
        /// Foreground and background
        /// </summary>
        private (Color, Color) UnCategorizedTransactionStateColoring = (Color.Black, Color.Yellow);

        public MoneyTrackingForm()
        {
            InitializeComponent();

            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            foreach (Category category in GetCategories())
            {
                categoryList.Items.Add(category);
            }

            LoadTransactions(TransactionFilePath);

            isActiveForm = true;
        }

        private void LoadTransactions(string path)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                var transactions = JsonConvert.DeserializeObject<IList<TransactionRecordJson>>(content);

                if (transactions != null && transactions.Any())
                {
                    transactionList.Items.Clear();
                    foreach (var transaction in transactions)
                    {
                        transactionList.Items.Add(transaction);
                    }
                    return;
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                statusLbl.Text = "Transaction file not found";
            }
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
                            Categories = new List<Category>() // Assuming you will handle categories separately
                        };

                        // Process each record
                        transactionList.Items.Add(transactionRecordJson);
                    }
                }
            }
            else
            {

            }
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
                var transactions = JsonConvert.SerializeObject(transactionList.Items, Formatting.Indented);
                File.WriteAllText(TransactionFilePath, transactions);
                return;
            }
            if (userWantsToReplaceTransactionFile)
            {
                File.WriteAllText(TransactionFilePath, string.Empty);
                var transactions = JsonConvert.SerializeObject(transactionList.Items, Formatting.Indented);
                File.WriteAllText(TransactionFilePath, transactions);
                MessageBox.Show("File has been replaced.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string content = File.ReadAllText(TransactionFilePath);

            var transactionsOld = JsonConvert.DeserializeObject<List<TransactionRecordJson>>(content) ?? new List<TransactionRecordJson>();

            for (int i = 0; i < transactionsOld.Count; i++)
            {
                foreach (TransactionRecordJson transactionNew in transactionList.Items)
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
            // Only process if an item is selected
            if (transactionList.SelectedIndex != -1)
            {
                // Display the last selected index (if valid)
                if (lastSelectedTransactionIndex != -1)
                {
                    // Category list refresh when user click away
                    for (int i = 0; i < categoryList.Items.Count; i++)
                    {
                        if (categoryList.GetItemChecked(i))
                        {
                            var category = categoryList.Items[i] as Category;
                            categoryList.SetItemChecked(i, false);
                            var record = (transactionList.Items[lastSelectedTransactionIndex] as TransactionRecordJson);
                            if (record != null)
                            {
                                if (!record.Categories.Any(x => x.Name == category.Name))
                                {
                                    record.Categories.Add(new Category(category.Name, true));
                                }
                            }
                        }
                    }

                    // Transaction handling when user click away
                    var transaction = transactionList.Items[transactionList.SelectedIndex] as TransactionRecordJson;

                    if (transaction != null)
                    {
                        foreach (var category in transaction.Categories)
                        {
                            for (int i = 0; i < categoryList.Items.Count; i++)
                            {
                                Category c = categoryList.Items[i] as Category;
                                if (c.Name == category.Name)
                                {
                                    c.IsChecked = true;
                                    categoryList.SetItemChecked(i, true);
                                }
                            }

                        }
                    }


                }
                // Update the last selected index
                lastSelectedTransactionIndex = transactionList.SelectedIndex;
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
    }
}
