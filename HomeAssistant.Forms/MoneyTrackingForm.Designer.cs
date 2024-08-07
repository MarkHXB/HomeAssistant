namespace HomeAssistant.Forms
{
    partial class MoneyTrackingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            label2 = new Label();
            searchTxtBox = new TextBox();
            shouldUpdateExportLbl = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            analyzeBtn = new Button();
            label1 = new Label();
            exportBtn = new Button();
            panel4 = new Panel();
            statusLbl = new Label();
            panel2 = new Panel();
            assignCategoriesBtn = new Button();
            categoryList = new CheckedListBox();
            addCategoryBtn = new Button();
            panel3 = new Panel();
            transactionList = new CheckedListBox();
            menuStrip1 = new MenuStrip();
            originalTransactionsToolStripMenuItem = new ToolStripMenuItem();
            categorizedTransactionsToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            contextMenuStrip1 = new ContextMenuStrip(components);
            renameToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            menuStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(searchTxtBox);
            panel1.Controls.Add(shouldUpdateExportLbl);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(analyzeBtn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(exportBtn);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 77);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(486, 53);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 7;
            label2.Text = "Search";
            // 
            // searchTxtBox
            // 
            searchTxtBox.Location = new Point(548, 51);
            searchTxtBox.Name = "searchTxtBox";
            searchTxtBox.Size = new Size(240, 23);
            searchTxtBox.TabIndex = 1;
            searchTxtBox.TextChanged += searchTxtBox_TextChanged;
            // 
            // shouldUpdateExportLbl
            // 
            shouldUpdateExportLbl.AutoSize = true;
            shouldUpdateExportLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            shouldUpdateExportLbl.ForeColor = Color.Red;
            shouldUpdateExportLbl.Location = new Point(497, 10);
            shouldUpdateExportLbl.Name = "shouldUpdateExportLbl";
            shouldUpdateExportLbl.Size = new Size(69, 25);
            shouldUpdateExportLbl.TabIndex = 6;
            shouldUpdateExportLbl.Text = "valami";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label5.Location = new Point(414, 14);
            label5.Name = "label5";
            label5.Size = new Size(22, 20);
            label5.TabIndex = 5;
            label5.Text = "3.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label4.Location = new Point(267, 14);
            label4.Name = "label4";
            label4.Size = new Size(22, 20);
            label4.TabIndex = 4;
            label4.Text = "2.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label3.Location = new Point(81, 25);
            label3.Name = "label3";
            label3.Size = new Size(22, 20);
            label3.TabIndex = 3;
            label3.Text = "1.";
            // 
            // analyzeBtn
            // 
            analyzeBtn.Location = new Point(391, 41);
            analyzeBtn.Name = "analyzeBtn";
            analyzeBtn.Size = new Size(75, 23);
            analyzeBtn.TabIndex = 2;
            analyzeBtn.Text = "Analye";
            analyzeBtn.UseVisualStyleBackColor = true;
            analyzeBtn.Click += analyzeBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 45);
            label1.Name = "label1";
            label1.Size = new Size(136, 15);
            label1.TabIndex = 1;
            label1.Text = "Current month payment";
            label1.Click += label1_Click;
            // 
            // exportBtn
            // 
            exportBtn.Location = new Point(232, 37);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(88, 31);
            exportBtn.TabIndex = 0;
            exportBtn.Text = "Export";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;
            // 
            // panel4
            // 
            panel4.Controls.Add(statusLbl);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 428);
            panel4.Name = "panel4";
            panel4.Size = new Size(800, 22);
            panel4.TabIndex = 3;
            // 
            // statusLbl
            // 
            statusLbl.AutoSize = true;
            statusLbl.Dock = DockStyle.Left;
            statusLbl.Location = new Point(0, 0);
            statusLbl.Name = "statusLbl";
            statusLbl.Padding = new Padding(30, 2, 0, 0);
            statusLbl.Size = new Size(68, 17);
            statusLbl.TabIndex = 0;
            statusLbl.Text = "label2";
            // 
            // panel2
            // 
            panel2.Controls.Add(assignCategoriesBtn);
            panel2.Controls.Add(categoryList);
            panel2.Controls.Add(addCategoryBtn);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 77);
            panel2.Name = "panel2";
            panel2.Size = new Size(191, 351);
            panel2.TabIndex = 4;
            // 
            // assignCategoriesBtn
            // 
            assignCategoriesBtn.BackColor = Color.FromArgb(224, 224, 224);
            assignCategoriesBtn.Dock = DockStyle.Bottom;
            assignCategoriesBtn.FlatAppearance.BorderColor = Color.White;
            assignCategoriesBtn.ForeColor = Color.Blue;
            assignCategoriesBtn.Location = new Point(0, 297);
            assignCategoriesBtn.Name = "assignCategoriesBtn";
            assignCategoriesBtn.Size = new Size(191, 27);
            assignCategoriesBtn.TabIndex = 2;
            assignCategoriesBtn.Text = "Assign categeroies";
            assignCategoriesBtn.UseVisualStyleBackColor = false;
            assignCategoriesBtn.Click += assignCategoriesBtn_Click;
            // 
            // categoryList
            // 
            categoryList.Dock = DockStyle.Fill;
            categoryList.FormattingEnabled = true;
            categoryList.Location = new Point(0, 0);
            categoryList.Name = "categoryList";
            categoryList.Size = new Size(191, 324);
            categoryList.TabIndex = 1;
            categoryList.SelectedIndexChanged += categoryList_SelectedIndexChanged;
            categoryList.MouseUp += categoryList_MouseUp;
            // 
            // addCategoryBtn
            // 
            addCategoryBtn.Dock = DockStyle.Bottom;
            addCategoryBtn.Location = new Point(0, 324);
            addCategoryBtn.Name = "addCategoryBtn";
            addCategoryBtn.Size = new Size(191, 27);
            addCategoryBtn.TabIndex = 0;
            addCategoryBtn.Text = "+ Add Category";
            addCategoryBtn.UseVisualStyleBackColor = true;
            addCategoryBtn.Click += addCategoryBtn_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(transactionList);
            panel3.Controls.Add(menuStrip1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(191, 77);
            panel3.Name = "panel3";
            panel3.Size = new Size(609, 351);
            panel3.TabIndex = 5;
            // 
            // transactionList
            // 
            transactionList.Dock = DockStyle.Fill;
            transactionList.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            transactionList.FormattingEnabled = true;
            transactionList.Location = new Point(0, 24);
            transactionList.Name = "transactionList";
            transactionList.Size = new Size(609, 327);
            transactionList.TabIndex = 0;
            transactionList.SelectedIndexChanged += transactionList_SelectedIndexChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { originalTransactionsToolStripMenuItem, categorizedTransactionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(609, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // originalTransactionsToolStripMenuItem
            // 
            originalTransactionsToolStripMenuItem.BackColor = Color.PapayaWhip;
            originalTransactionsToolStripMenuItem.Name = "originalTransactionsToolStripMenuItem";
            originalTransactionsToolStripMenuItem.Size = new Size(129, 20);
            originalTransactionsToolStripMenuItem.Text = "Original Transactions";
            originalTransactionsToolStripMenuItem.Click += originalTransactionsToolStripMenuItem_Click;
            // 
            // categorizedTransactionsToolStripMenuItem
            // 
            categorizedTransactionsToolStripMenuItem.BackColor = Color.LightBlue;
            categorizedTransactionsToolStripMenuItem.Name = "categorizedTransactionsToolStripMenuItem";
            categorizedTransactionsToolStripMenuItem.Size = new Size(150, 20);
            categorizedTransactionsToolStripMenuItem.Text = "Categorized Transactions";
            categorizedTransactionsToolStripMenuItem.Click += categorizedTransactionsToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { renameToolStripMenuItem, deleteToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(118, 48);
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new Size(117, 22);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(117, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // MoneyTrackingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel4);
            Controls.Add(panel1);
            MainMenuStrip = menuStrip1;
            Name = "MoneyTrackingForm";
            Text = "MoneyTrackingForm";
            FormClosed += MoneyTrackingForm_FormClosed;
            VisibleChanged += MoneyTrackingForm_VisibleChanged;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button exportBtn;
        private Panel panel4;
        private Label statusLbl;
        private Panel panel2;
        private Panel panel3;
        private Button analyzeBtn;
        private Label shouldUpdateExportLbl;
        private Label label5;
        private Label label4;
        private Label label3;
        private CheckedListBox categoryList;
        private Button addCategoryBtn;
        private OpenFileDialog openFileDialog1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private CheckedListBox transactionList;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox searchTxtBox;
        private Button assignCategoriesBtn;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem originalTransactionsToolStripMenuItem;
        private ToolStripMenuItem categorizedTransactionsToolStripMenuItem;
    }
}