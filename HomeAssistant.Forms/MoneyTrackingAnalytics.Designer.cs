namespace HomeAssistant.Forms
{
    partial class MoneyTrackingAnalytics
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
            backBtn = new Button();
            panel1 = new Panel();
            label1 = new Label();
            label3 = new Label();
            historyListBox = new ListBox();
            label4 = new Label();
            incomeLbl = new Label();
            savingLbl = new Label();
            outcomeLbl = new Label();
            label8 = new Label();
            label9 = new Label();
            addGoalBtn = new Button();
            statusLbl = new Label();
            goalsList = new ListBox();
            addSavingToCurrentMonthBtn = new Button();
            savingsChart = new LiveChartsCore.SkiaSharpView.WinForms.PieChart();
            balanceChart = new LiveChartsCore.SkiaSharpView.WinForms.PieChart();
            savingForGoalLbl = new Label();
            label2 = new Label();
            addAutomaticSavingPerMBtn = new Button();
            balanceGoalsFilterChkBox = new CheckBox();
            balanceGoalsAutoFilterChkBox = new CheckBox();
            maxOutcomeChart = new LiveChartsCore.SkiaSharpView.WinForms.PieChart();
            maxOutcomeLbl = new Label();
            modifyMaxOutcomeBtn = new Button();
            panel4 = new Panel();
            label6 = new Label();
            cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            label5 = new Label();
            SuspendLayout();
            // 
            // backBtn
            // 
            backBtn.FlatAppearance.BorderColor = Color.LightSeaGreen;
            backBtn.FlatAppearance.BorderSize = 3;
            backBtn.FlatStyle = FlatStyle.Flat;
            backBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            backBtn.Location = new Point(12, 12);
            backBtn.Name = "backBtn";
            backBtn.Size = new Size(83, 32);
            backBtn.TabIndex = 1;
            backBtn.Text = "Back";
            backBtn.UseVisualStyleBackColor = true;
            backBtn.Click += backBtn_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(64, 64, 64);
            panel1.Location = new Point(43, 502);
            panel1.Name = "panel1";
            panel1.Size = new Size(1279, 3);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label1.Location = new Point(96, 524);
            label1.Name = "label1";
            label1.Size = new Size(74, 25);
            label1.TabIndex = 5;
            label1.Text = "Income";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label3.Location = new Point(96, 569);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 7;
            label3.Text = "Outcome";
            // 
            // historyListBox
            // 
            historyListBox.FormattingEnabled = true;
            historyListBox.ItemHeight = 15;
            historyListBox.Location = new Point(1108, 58);
            historyListBox.Name = "historyListBox";
            historyListBox.Size = new Size(214, 244);
            historyListBox.TabIndex = 8;
            historyListBox.SelectedIndexChanged += historyListBox_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label4.Location = new Point(1163, 19);
            label4.Name = "label4";
            label4.Size = new Size(71, 25);
            label4.TabIndex = 9;
            label4.Text = "History";
            // 
            // incomeLbl
            // 
            incomeLbl.AutoSize = true;
            incomeLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            incomeLbl.ForeColor = Color.Green;
            incomeLbl.Location = new Point(196, 524);
            incomeLbl.Name = "incomeLbl";
            incomeLbl.Size = new Size(58, 25);
            incomeLbl.TabIndex = 11;
            incomeLbl.Text = "+123";
            // 
            // savingLbl
            // 
            savingLbl.AutoSize = true;
            savingLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            savingLbl.ForeColor = Color.Green;
            savingLbl.Location = new Point(1264, 968);
            savingLbl.Name = "savingLbl";
            savingLbl.Size = new Size(58, 25);
            savingLbl.TabIndex = 12;
            savingLbl.Text = "+123";
            // 
            // outcomeLbl
            // 
            outcomeLbl.AutoSize = true;
            outcomeLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            outcomeLbl.ForeColor = Color.Red;
            outcomeLbl.Location = new Point(196, 569);
            outcomeLbl.Name = "outcomeLbl";
            outcomeLbl.Size = new Size(53, 25);
            outcomeLbl.TabIndex = 13;
            outcomeLbl.Text = "-123";
            outcomeLbl.Click += label7_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label8.Location = new Point(96, 156);
            label8.Name = "label8";
            label8.Size = new Size(0, 18);
            label8.TabIndex = 14;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label9.Location = new Point(1082, 968);
            label9.Name = "label9";
            label9.Size = new Size(163, 25);
            label9.TabIndex = 16;
            label9.Text = "Reamining money";
            // 
            // addGoalBtn
            // 
            addGoalBtn.FlatAppearance.BorderColor = Color.LightSeaGreen;
            addGoalBtn.FlatAppearance.BorderSize = 3;
            addGoalBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            addGoalBtn.Location = new Point(943, 578);
            addGoalBtn.Name = "addGoalBtn";
            addGoalBtn.Size = new Size(124, 32);
            addGoalBtn.TabIndex = 17;
            addGoalBtn.Text = "+ Add goal";
            addGoalBtn.UseVisualStyleBackColor = true;
            addGoalBtn.Click += addGoalBtn_Click;
            // 
            // statusLbl
            // 
            statusLbl.AutoSize = true;
            statusLbl.Dock = DockStyle.Bottom;
            statusLbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 238);
            statusLbl.Location = new Point(0, 982);
            statusLbl.Name = "statusLbl";
            statusLbl.Size = new Size(0, 20);
            statusLbl.TabIndex = 18;
            // 
            // goalsList
            // 
            goalsList.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            goalsList.FormattingEnabled = true;
            goalsList.ItemHeight = 20;
            goalsList.Location = new Point(1082, 524);
            goalsList.Name = "goalsList";
            goalsList.Size = new Size(228, 124);
            goalsList.TabIndex = 19;
            goalsList.SelectedIndexChanged += goalsList_SelectedIndexChanged;
            // 
            // addSavingToCurrentMonthBtn
            // 
            addSavingToCurrentMonthBtn.FlatAppearance.BorderColor = Color.LightSeaGreen;
            addSavingToCurrentMonthBtn.FlatAppearance.BorderSize = 3;
            addSavingToCurrentMonthBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            addSavingToCurrentMonthBtn.Location = new Point(825, 616);
            addSavingToCurrentMonthBtn.Name = "addSavingToCurrentMonthBtn";
            addSavingToCurrentMonthBtn.Size = new Size(248, 32);
            addSavingToCurrentMonthBtn.TabIndex = 20;
            addSavingToCurrentMonthBtn.Text = "+ Add saving for the current month";
            addSavingToCurrentMonthBtn.UseVisualStyleBackColor = true;
            addSavingToCurrentMonthBtn.Click += addSavingToCurrentMonthBtn_Click;
            // 
            // savingsChart
            // 
            savingsChart.InitialRotation = 0D;
            savingsChart.IsClockwise = true;
            savingsChart.Location = new Point(1014, 740);
            savingsChart.MaxAngle = 360D;
            savingsChart.MaxValue = null;
            savingsChart.MinValue = 0D;
            savingsChart.Name = "savingsChart";
            savingsChart.Size = new Size(308, 184);
            savingsChart.TabIndex = 22;
            savingsChart.Total = null;
            // 
            // balanceChart
            // 
            balanceChart.InitialRotation = 0D;
            balanceChart.IsClockwise = true;
            balanceChart.Location = new Point(222, 58);
            balanceChart.MaxAngle = 360D;
            balanceChart.MaxValue = null;
            balanceChart.MinValue = 0D;
            balanceChart.Name = "balanceChart";
            balanceChart.Size = new Size(600, 401);
            balanceChart.TabIndex = 23;
            balanceChart.Total = null;
            // 
            // savingForGoalLbl
            // 
            savingForGoalLbl.AutoSize = true;
            savingForGoalLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            savingForGoalLbl.Location = new Point(1087, 683);
            savingForGoalLbl.Name = "savingForGoalLbl";
            savingForGoalLbl.Size = new Size(147, 25);
            savingForGoalLbl.TabIndex = 24;
            savingForGoalLbl.Text = "Saving for goals";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label2.Location = new Point(513, 19);
            label2.Name = "label2";
            label2.Size = new Size(78, 25);
            label2.TabIndex = 25;
            label2.Text = "Balance";
            // 
            // addAutomaticSavingPerMBtn
            // 
            addAutomaticSavingPerMBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 0);
            addAutomaticSavingPerMBtn.FlatAppearance.BorderSize = 3;
            addAutomaticSavingPerMBtn.FlatStyle = FlatStyle.Flat;
            addAutomaticSavingPerMBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            addAutomaticSavingPerMBtn.Location = new Point(498, 552);
            addAutomaticSavingPerMBtn.Name = "addAutomaticSavingPerMBtn";
            addAutomaticSavingPerMBtn.Size = new Size(208, 73);
            addAutomaticSavingPerMBtn.TabIndex = 26;
            addAutomaticSavingPerMBtn.Text = "Add automatic saving per month";
            addAutomaticSavingPerMBtn.UseVisualStyleBackColor = true;
            addAutomaticSavingPerMBtn.Click += addAutomaticSavingPerMBtn_Click;
            // 
            // balanceGoalsFilterChkBox
            // 
            balanceGoalsFilterChkBox.AutoSize = true;
            balanceGoalsFilterChkBox.Location = new Point(943, 94);
            balanceGoalsFilterChkBox.Name = "balanceGoalsFilterChkBox";
            balanceGoalsFilterChkBox.Size = new Size(96, 19);
            balanceGoalsFilterChkBox.TabIndex = 27;
            balanceGoalsFilterChkBox.Text = "Include goals";
            balanceGoalsFilterChkBox.UseVisualStyleBackColor = true;
            balanceGoalsFilterChkBox.CheckedChanged += balanceGoalsFilterChkBox_CheckedChanged;
            // 
            // balanceGoalsAutoFilterChkBox
            // 
            balanceGoalsAutoFilterChkBox.AutoSize = true;
            balanceGoalsAutoFilterChkBox.Location = new Point(907, 119);
            balanceGoalsAutoFilterChkBox.Name = "balanceGoalsAutoFilterChkBox";
            balanceGoalsAutoFilterChkBox.Size = new Size(132, 19);
            balanceGoalsAutoFilterChkBox.TabIndex = 28;
            balanceGoalsAutoFilterChkBox.Text = "Include goal ( auto )";
            balanceGoalsAutoFilterChkBox.UseVisualStyleBackColor = true;
            balanceGoalsAutoFilterChkBox.CheckedChanged += balanceGoalsAutoFilterChkBox_CheckedChanged;
            // 
            // maxOutcomeChart
            // 
            maxOutcomeChart.InitialRotation = 0D;
            maxOutcomeChart.IsClockwise = true;
            maxOutcomeChart.Location = new Point(61, 721);
            maxOutcomeChart.MaxAngle = 360D;
            maxOutcomeChart.MaxValue = null;
            maxOutcomeChart.MinValue = 0D;
            maxOutcomeChart.Name = "maxOutcomeChart";
            maxOutcomeChart.Size = new Size(346, 203);
            maxOutcomeChart.TabIndex = 30;
            maxOutcomeChart.Total = null;
            // 
            // maxOutcomeLbl
            // 
            maxOutcomeLbl.AutoSize = true;
            maxOutcomeLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            maxOutcomeLbl.Location = new Point(176, 683);
            maxOutcomeLbl.Name = "maxOutcomeLbl";
            maxOutcomeLbl.Size = new Size(127, 25);
            maxOutcomeLbl.TabIndex = 31;
            maxOutcomeLbl.Text = "Max outcome";
            // 
            // modifyMaxOutcomeBtn
            // 
            modifyMaxOutcomeBtn.FlatAppearance.BorderColor = Color.LightSeaGreen;
            modifyMaxOutcomeBtn.FlatAppearance.BorderSize = 3;
            modifyMaxOutcomeBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            modifyMaxOutcomeBtn.Location = new Point(115, 930);
            modifyMaxOutcomeBtn.Name = "modifyMaxOutcomeBtn";
            modifyMaxOutcomeBtn.Size = new Size(248, 32);
            modifyMaxOutcomeBtn.TabIndex = 32;
            modifyMaxOutcomeBtn.Text = "Modify max outcome";
            modifyMaxOutcomeBtn.UseVisualStyleBackColor = true;
            modifyMaxOutcomeBtn.Click += modifyMaxOutcomeBtn_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(64, 64, 64);
            panel4.Location = new Point(43, 659);
            panel4.Name = "panel4";
            panel4.Size = new Size(1279, 3);
            panel4.TabIndex = 3;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label6.Location = new Point(534, 524);
            label6.Name = "label6";
            label6.Size = new Size(133, 25);
            label6.TabIndex = 33;
            label6.Text = "Saving Overall";
            // 
            // cartesianChart1
            // 
            cartesianChart1.AutoScroll = true;
            cartesianChart1.Location = new Point(431, 721);
            cartesianChart1.Name = "cartesianChart1";
            cartesianChart1.Size = new Size(556, 257);
            cartesianChart1.TabIndex = 34;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label5.Location = new Point(658, 683);
            label5.Name = "label5";
            label5.Size = new Size(127, 25);
            label5.TabIndex = 35;
            label5.Text = "By Categories";
            // 
            // MoneyTrackingAnalytics
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1331, 1002);
            Controls.Add(label5);
            Controls.Add(cartesianChart1);
            Controls.Add(label6);
            Controls.Add(panel4);
            Controls.Add(modifyMaxOutcomeBtn);
            Controls.Add(maxOutcomeLbl);
            Controls.Add(maxOutcomeChart);
            Controls.Add(balanceGoalsAutoFilterChkBox);
            Controls.Add(balanceGoalsFilterChkBox);
            Controls.Add(addAutomaticSavingPerMBtn);
            Controls.Add(label2);
            Controls.Add(savingForGoalLbl);
            Controls.Add(balanceChart);
            Controls.Add(savingsChart);
            Controls.Add(addSavingToCurrentMonthBtn);
            Controls.Add(goalsList);
            Controls.Add(statusLbl);
            Controls.Add(addGoalBtn);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(outcomeLbl);
            Controls.Add(savingLbl);
            Controls.Add(incomeLbl);
            Controls.Add(label4);
            Controls.Add(historyListBox);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(panel1);
            Controls.Add(backBtn);
            Name = "MoneyTrackingAnalytics";
            Text = "MoneyTrackingAnalytics";
            Load += MoneyTrackingAnalytics_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button backBtn;
        private Panel panel1;
        private Label label1;
        private Label label3;
        private ListBox historyListBox;
        private Label label4;
        private Label incomeLbl;
        private Label savingLbl;
        private Label outcomeLbl;
        private Label label8;
        private Label label9;
        private Button addGoalBtn;
        private Label statusLbl;
        private ListBox goalsList;
        private Button addSavingToCurrentMonthBtn;
        private LiveChartsCore.SkiaSharpView.WinForms.PieChart savingsChart;
        private LiveChartsCore.SkiaSharpView.WinForms.PieChart balanceChart;
        private Label savingForGoalLbl;
        private Label label2;
        private Button addAutomaticSavingPerMBtn;
        private CheckBox balanceGoalsFilterChkBox;
        private CheckBox balanceGoalsAutoFilterChkBox;
        private LiveChartsCore.SkiaSharpView.WinForms.PieChart maxOutcomeChart;
        private Label label6;
        private Button modifyMaxOutcomeBtn;
        private Label maxOutcomeLbl;
        private Panel panel4;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
        private Label label5;
    }
}