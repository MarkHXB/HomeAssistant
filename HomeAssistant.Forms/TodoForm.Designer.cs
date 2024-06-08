namespace HomeAssistant.Forms
{
    partial class TodoForm
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
            todosList = new ListBox();
            todoTitleTxtBox = new TextBox();
            todoTitleLbl = new Label();
            todoDueDateLbl = new Label();
            todoDueDateTxtBox = new TextBox();
            todoReminderDateLbl = new Label();
            todoReminderDateTxtBox = new TextBox();
            todoIsCompletedChckBox = new CheckBox();
            todoIsReminderDateChckBox = new CheckBox();
            todoIsNotifiedDueDateChckBox = new CheckBox();
            saveBtn = new Button();
            updatePctBx = new PictureBox();
            updatLbl = new Label();
            addNewTodoBtn = new Button();
            fader1 = new NAudio.Gui.Fader();
            deleteTodoBtn = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)updatePctBx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deleteTodoBtn).BeginInit();
            SuspendLayout();
            // 
            // todosList
            // 
            todosList.DrawMode = DrawMode.OwnerDrawFixed;
            todosList.FormattingEnabled = true;
            todosList.ItemHeight = 15;
            todosList.Location = new Point(415, 42);
            todosList.Name = "todosList";
            todosList.Size = new Size(373, 394);
            todosList.TabIndex = 0;
            todosList.DrawItem += todosList_DrawItem;
            todosList.SelectedIndexChanged += todosList_SelectedIndexChanged;
            // 
            // todoTitleTxtBox
            // 
            todoTitleTxtBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            todoTitleTxtBox.Location = new Point(30, 54);
            todoTitleTxtBox.Name = "todoTitleTxtBox";
            todoTitleTxtBox.Size = new Size(331, 33);
            todoTitleTxtBox.TabIndex = 1;
            todoTitleTxtBox.TextChanged += todoTitleTxtBox_TextChanged;
            // 
            // todoTitleLbl
            // 
            todoTitleLbl.AutoSize = true;
            todoTitleLbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoTitleLbl.Location = new Point(30, 39);
            todoTitleLbl.Name = "todoTitleLbl";
            todoTitleLbl.Size = new Size(44, 21);
            todoTitleLbl.TabIndex = 2;
            todoTitleLbl.Text = "Title";
            // 
            // todoDueDateLbl
            // 
            todoDueDateLbl.AutoSize = true;
            todoDueDateLbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoDueDateLbl.Location = new Point(30, 116);
            todoDueDateLbl.Name = "todoDueDateLbl";
            todoDueDateLbl.Size = new Size(81, 21);
            todoDueDateLbl.TabIndex = 4;
            todoDueDateLbl.Text = "Due Date";
            // 
            // todoDueDateTxtBox
            // 
            todoDueDateTxtBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            todoDueDateTxtBox.Location = new Point(30, 131);
            todoDueDateTxtBox.Name = "todoDueDateTxtBox";
            todoDueDateTxtBox.Size = new Size(331, 33);
            todoDueDateTxtBox.TabIndex = 3;
            // 
            // todoReminderDateLbl
            // 
            todoReminderDateLbl.AutoSize = true;
            todoReminderDateLbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoReminderDateLbl.Location = new Point(30, 188);
            todoReminderDateLbl.Name = "todoReminderDateLbl";
            todoReminderDateLbl.Size = new Size(124, 21);
            todoReminderDateLbl.TabIndex = 6;
            todoReminderDateLbl.Text = "Reminder Date";
            // 
            // todoReminderDateTxtBox
            // 
            todoReminderDateTxtBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            todoReminderDateTxtBox.Location = new Point(30, 203);
            todoReminderDateTxtBox.Name = "todoReminderDateTxtBox";
            todoReminderDateTxtBox.Size = new Size(331, 33);
            todoReminderDateTxtBox.TabIndex = 5;
            // 
            // todoIsCompletedChckBox
            // 
            todoIsCompletedChckBox.AutoSize = true;
            todoIsCompletedChckBox.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoIsCompletedChckBox.Location = new Point(30, 262);
            todoIsCompletedChckBox.Name = "todoIsCompletedChckBox";
            todoIsCompletedChckBox.Size = new Size(129, 25);
            todoIsCompletedChckBox.TabIndex = 7;
            todoIsCompletedChckBox.Text = "Is Completed";
            todoIsCompletedChckBox.UseVisualStyleBackColor = true;
            // 
            // todoIsReminderDateChckBox
            // 
            todoIsReminderDateChckBox.AutoSize = true;
            todoIsReminderDateChckBox.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoIsReminderDateChckBox.Location = new Point(30, 362);
            todoIsReminderDateChckBox.Name = "todoIsReminderDateChckBox";
            todoIsReminderDateChckBox.Size = new Size(184, 25);
            todoIsReminderDateChckBox.TabIndex = 8;
            todoIsReminderDateChckBox.Text = "Is Notified Due Date";
            todoIsReminderDateChckBox.UseVisualStyleBackColor = true;
            // 
            // todoIsNotifiedDueDateChckBox
            // 
            todoIsNotifiedDueDateChckBox.AutoSize = true;
            todoIsNotifiedDueDateChckBox.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            todoIsNotifiedDueDateChckBox.Location = new Point(30, 313);
            todoIsNotifiedDueDateChckBox.Name = "todoIsNotifiedDueDateChckBox";
            todoIsNotifiedDueDateChckBox.Size = new Size(227, 25);
            todoIsNotifiedDueDateChckBox.TabIndex = 9;
            todoIsNotifiedDueDateChckBox.Text = "Is Notified Reminder Date";
            todoIsNotifiedDueDateChckBox.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            saveBtn.BackColor = Color.Blue;
            saveBtn.FlatAppearance.BorderSize = 0;
            saveBtn.FlatStyle = FlatStyle.Flat;
            saveBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            saveBtn.ForeColor = Color.White;
            saveBtn.Location = new Point(248, 403);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(124, 33);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "Save";
            saveBtn.UseVisualStyleBackColor = false;
            saveBtn.Click += saveBtn_Click;
            // 
            // updatePctBx
            // 
            updatePctBx.Image = Resource1.refresh_page_option;
            updatePctBx.Location = new Point(415, 10);
            updatePctBx.Name = "updatePctBx";
            updatePctBx.Size = new Size(22, 23);
            updatePctBx.SizeMode = PictureBoxSizeMode.StretchImage;
            updatePctBx.TabIndex = 11;
            updatePctBx.TabStop = false;
            updatePctBx.Click += updatePctBx_Click;
            // 
            // updatLbl
            // 
            updatLbl.AutoSize = true;
            updatLbl.Location = new Point(453, 18);
            updatLbl.Name = "updatLbl";
            updatLbl.Size = new Size(68, 15);
            updatLbl.TabIndex = 12;
            updatLbl.Text = "Updated at:";
            // 
            // addNewTodoBtn
            // 
            addNewTodoBtn.BackColor = Color.DarkCyan;
            addNewTodoBtn.FlatAppearance.BorderSize = 0;
            addNewTodoBtn.FlatStyle = FlatStyle.Flat;
            addNewTodoBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            addNewTodoBtn.ForeColor = Color.White;
            addNewTodoBtn.Location = new Point(30, 403);
            addNewTodoBtn.Name = "addNewTodoBtn";
            addNewTodoBtn.Size = new Size(124, 33);
            addNewTodoBtn.TabIndex = 14;
            addNewTodoBtn.Text = "Add";
            addNewTodoBtn.UseVisualStyleBackColor = false;
            addNewTodoBtn.Click += addNewTodoBtn_Click;
            // 
            // fader1
            // 
            fader1.Location = new Point(334, 381);
            fader1.Maximum = 0;
            fader1.Minimum = 0;
            fader1.Name = "fader1";
            fader1.Orientation = Orientation.Horizontal;
            fader1.Size = new Size(75, 23);
            fader1.TabIndex = 15;
            fader1.Text = "fader1";
            fader1.Value = int.MinValue;
            // 
            // deleteTodoBtn
            // 
            deleteTodoBtn.Image = Resource1.trash_can;
            deleteTodoBtn.Location = new Point(766, 10);
            deleteTodoBtn.Name = "deleteTodoBtn";
            deleteTodoBtn.Size = new Size(22, 23);
            deleteTodoBtn.SizeMode = PictureBoxSizeMode.StretchImage;
            deleteTodoBtn.TabIndex = 16;
            deleteTodoBtn.TabStop = false;
            deleteTodoBtn.Click += deleteTodoBtn_Click;
            // 
            // TodoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(deleteTodoBtn);
            Controls.Add(fader1);
            Controls.Add(addNewTodoBtn);
            Controls.Add(updatLbl);
            Controls.Add(updatePctBx);
            Controls.Add(saveBtn);
            Controls.Add(todoIsNotifiedDueDateChckBox);
            Controls.Add(todoIsReminderDateChckBox);
            Controls.Add(todoIsCompletedChckBox);
            Controls.Add(todoReminderDateLbl);
            Controls.Add(todoReminderDateTxtBox);
            Controls.Add(todoDueDateLbl);
            Controls.Add(todoDueDateTxtBox);
            Controls.Add(todoTitleLbl);
            Controls.Add(todoTitleTxtBox);
            Controls.Add(todosList);
            Name = "TodoForm";
            Text = "TodoForm";
            Load += TodoForm_Load;
            ((System.ComponentModel.ISupportInitialize)updatePctBx).EndInit();
            ((System.ComponentModel.ISupportInitialize)deleteTodoBtn).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox todosList;
        private TextBox todoTitleTxtBox;
        private Label todoTitleLbl;
        private Label todoDueDateLbl;
        private TextBox todoDueDateTxtBox;
        private Label todoReminderDateLbl;
        private TextBox todoReminderDateTxtBox;
        private CheckBox todoIsCompletedChckBox;
        private CheckBox todoIsReminderDateChckBox;
        private CheckBox todoIsNotifiedDueDateChckBox;
        private Button saveBtn;
        private PictureBox updatePctBx;
        private Label updatLbl;
        private Button addNewTodoBtn;
        private NAudio.Gui.Fader fader1;
        private PictureBox deleteTodoBtn;
    }
}