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
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            backBtn = new Button();
            panel1 = new Panel();
            formsPlot2 = new ScottPlot.WinForms.FormsPlot();
            formsPlot3 = new ScottPlot.WinForms.FormsPlot();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            listBox1 = new ListBox();
            label4 = new Label();
            incomeLbl = new Label();
            label6 = new Label();
            outcomeLbl = new Label();
            label8 = new Label();
            panel2 = new Panel();
            checkedListBox1 = new CheckedListBox();
            label9 = new Label();
            button2 = new Button();
            statusLbl = new Label();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1F;
            formsPlot1.Location = new Point(52, 326);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(287, 203);
            formsPlot1.TabIndex = 0;
            formsPlot1.Load += formsPlot1_Load;
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
            panel1.Location = new Point(43, 281);
            panel1.Name = "panel1";
            panel1.Size = new Size(1279, 3);
            panel1.TabIndex = 2;
            // 
            // formsPlot2
            // 
            formsPlot2.DisplayScale = 1F;
            formsPlot2.Location = new Point(409, 302);
            formsPlot2.Name = "formsPlot2";
            formsPlot2.Size = new Size(518, 258);
            formsPlot2.TabIndex = 3;
            // 
            // formsPlot3
            // 
            formsPlot3.DisplayScale = 1F;
            formsPlot3.Location = new Point(991, 326);
            formsPlot3.Name = "formsPlot3";
            formsPlot3.Size = new Size(331, 203);
            formsPlot3.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label1.Location = new Point(165, 302);
            label1.Name = "label1";
            label1.Size = new Size(74, 25);
            label1.TabIndex = 5;
            label1.Text = "Income";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label2.Location = new Point(613, 287);
            label2.Name = "label2";
            label2.Size = new Size(129, 25);
            label2.TabIndex = 6;
            label2.Text = "Planed saving";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label3.Location = new Point(1124, 302);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 7;
            label3.Text = "Outcome";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(1093, 80);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(214, 169);
            listBox1.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label4.Location = new Point(1163, 40);
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
            incomeLbl.Location = new Point(165, 535);
            incomeLbl.Name = "incomeLbl";
            incomeLbl.Size = new Size(58, 25);
            incomeLbl.TabIndex = 11;
            incomeLbl.Text = "+123";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label6.ForeColor = Color.Green;
            label6.Location = new Point(646, 551);
            label6.Name = "label6";
            label6.Size = new Size(58, 25);
            label6.TabIndex = 12;
            label6.Text = "+123";
            // 
            // outcomeLbl
            // 
            outcomeLbl.AutoSize = true;
            outcomeLbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            outcomeLbl.ForeColor = Color.Red;
            outcomeLbl.Location = new Point(1142, 532);
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
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(64, 64, 64);
            panel2.Location = new Point(1050, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(3, 250);
            panel2.TabIndex = 3;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(185, 58);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(192, 166);
            checkedListBox1.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label9.Location = new Point(203, 30);
            label9.Name = "label9";
            label9.Size = new Size(147, 25);
            label9.TabIndex = 16;
            label9.Text = "Saving for goals";
            // 
            // button2
            // 
            button2.FlatAppearance.BorderColor = Color.LightSeaGreen;
            button2.FlatAppearance.BorderSize = 3;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button2.Location = new Point(215, 230);
            button2.Name = "button2";
            button2.Size = new Size(124, 32);
            button2.TabIndex = 17;
            button2.Text = "+ Add goal";
            button2.UseVisualStyleBackColor = true;
            // 
            // statusLbl
            // 
            statusLbl.AutoSize = true;
            statusLbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 238);
            statusLbl.Location = new Point(12, 589);
            statusLbl.Name = "statusLbl";
            statusLbl.Size = new Size(0, 20);
            statusLbl.TabIndex = 18;
            // 
            // MoneyTrackingAnalytics
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1355, 623);
            Controls.Add(statusLbl);
            Controls.Add(button2);
            Controls.Add(label9);
            Controls.Add(checkedListBox1);
            Controls.Add(panel2);
            Controls.Add(label8);
            Controls.Add(outcomeLbl);
            Controls.Add(label6);
            Controls.Add(incomeLbl);
            Controls.Add(label4);
            Controls.Add(listBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(formsPlot3);
            Controls.Add(formsPlot2);
            Controls.Add(panel1);
            Controls.Add(backBtn);
            Controls.Add(formsPlot1);
            Name = "MoneyTrackingAnalytics";
            Text = "MoneyTrackingAnalytics";
            Load += MoneyTrackingAnalytics_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button backBtn;
        private Panel panel1;
        private ScottPlot.WinForms.FormsPlot formsPlot2;
        private ScottPlot.WinForms.FormsPlot formsPlot3;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListBox listBox1;
        private Label label4;
        private Label incomeLbl;
        private Label label6;
        private Label outcomeLbl;
        private Label label8;
        private Panel panel2;
        private CheckedListBox checkedListBox1;
        private Label label9;
        private Button button2;
        private Label statusLbl;
    }
}