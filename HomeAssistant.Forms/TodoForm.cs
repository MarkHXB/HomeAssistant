using HomeAssistant.Lib.Subsystems.Todo;
using SubSystemComponent;
using Todo;

namespace HomeAssistant.Forms
{
    public partial class TodoForm : Form
    {
        private IEnumerable<TodoItem> _items;
        private TodoItem? _selectedItem = null;
        private Subsystem _todoSystem;

        private const string UpdateLabelText = "Updated at: ";

        private string TodoTitle => todoTitleTxtBox.Text;
        private string TodoReminderDate => todoReminderDateTxtBox.Text;
        private string TodoDueDate => todoDueDateTxtBox.Text;


        public TodoForm()
        {
            _todoSystem = null;

            InitializeComponent();
        }

        private void TodoForm_Load(object sender, EventArgs e)
        {
            InitSubSystem(TodoCommand.GETALL);

            //LoadTodos();
        }
        private void InitSubSystem(TodoCommand todoCommand)
        {
            Dictionary<string, string> @params = new Dictionary<string, string>();

            switch (todoCommand)
            {
                case TodoCommand.ADD:
                    @params.Add("todo_system_command", "ADD");
                    @params.Add("todo_system_new_item_title", todoTitleTxtBox.Text);
                    @params.Add("todo_system_new_item_duetodate", todoDueDateTxtBox.Text);
                    @params.Add("todo_system_new_item_reminderdate", todoReminderDateTxtBox.Text);
                    break;
                case TodoCommand.UPDATE:
                    @params.Add("todo_system_command", "UPDATE");
                    @params.Add("todo_system_id", _selectedItem.Id);
                    @params.Add("todo_system_new_item_title", todoTitleTxtBox.Text);
                    @params.Add("todo_system_new_item_duetodate", todoDueDateTxtBox.Text);
                    @params.Add("todo_system_new_item_reminderdate", todoReminderDateTxtBox.Text);
                    @params.Add("todo_system_new_item_iscompleted", todoIsCompletedChckBox.Checked ? "true" : "false");
                    break;
                case TodoCommand.GETALL:
                    @params.Add("todo_system_command", "GETALL");
                    break;
                case TodoCommand.DELETE:
                    @params.Add("todo_system_command", "DELETE");
                    @params.Add("todo_system_id", _selectedItem.Id);
                    break;
                default:
                    break;
            }

            //{"todo_system_new_item_title", "Test2"},
            //{ "todo_system_new_item_duetodate", "2024.06.06."},
            //{"todo_system_new_item_reminderdate", "2024.06.05."},
            //{"todo_system_new_item_iscompleted", "fa"},


            _todoSystem = SubSystemFactory<TodoSystem>.Create(@params);

            SubsystemPool.AddSubsystem(_todoSystem);
        }
        private void LoadTodos()
        {
            todosList.Items.Clear();

            foreach (var todo in _items)
            {
                todosList.Items.Add(todo);
            }
        }

        private void LoadTodo()
        {
            if (_selectedItem == null)
            {
                return;
            }

            todoTitleTxtBox.Text = _selectedItem.Title;
            todoDueDateTxtBox.Text = _selectedItem.DueDate.ToString();
            todoReminderDateTxtBox.Text = _selectedItem.ReminderDate.ToString();
            todoIsCompletedChckBox.Checked = _selectedItem.IsCompleted;
            todoIsNotifiedDueDateChckBox.Checked = _selectedItem.IsNotifiedDueDate;
            todoIsReminderDateChckBox.Checked = _selectedItem.IsNotifiedReminderDate;
        }

        private void HighlightOutOfDateTodos(Control control)
        {

        }

        private void todoTitleTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void todosList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedItem = todosList.SelectedItem as TodoItem;

            LoadTodo();
        }

        private async void updatePctBx_Click(object sender, EventArgs e)
        {
            await RefreshTodos();
        }

        private async Task RefreshTodos()
        {
            InitSubSystem(TodoCommand.GETALL);

            await SubsystemPool.RunAllAsync(new CancellationToken());

            _items = _todoSystem.GetOutput<IEnumerable<TodoItem>>(false);

            LoadTodos();

            updatLbl.Text = UpdateLabelText + DateTime.Now.ToString();
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            InitSubSystem(TodoCommand.UPDATE);

            await SubsystemPool.RunAllAsync(new CancellationToken());

            await RefreshTodos();
        }

        private async void addNewTodoBtn_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                InitSubSystem(TodoCommand.ADD);

                await SubsystemPool.RunAllAsync(new CancellationToken());

                await RefreshTodos();
            }
        }

        private bool IsValidForm()
        {
            if (string.IsNullOrWhiteSpace(TodoTitle))
            {
                MessageBox.Show("Title is incorrect");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(TodoReminderDate))
            {
                DateTime.TryParse(TodoReminderDate, out DateTime reminderDate);

                if (reminderDate == DateTime.MinValue)
                {
                    MessageBox.Show("Reminder date is incorrect");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Reminder date is incorrect");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(TodoDueDate))
            {
                DateTime.TryParse(TodoDueDate, out DateTime duedate);

                if (duedate == DateTime.MinValue)
                {
                    MessageBox.Show("Due date is incorrect");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Due date is incorrect");
                return false;
            }

            return true;
        }

        private async void deleteTodoBtn_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("You must select a todo to delete it"); return;
            }

            // Display a message box with Yes and No buttons
            var result = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // Check the result of the message box
            if (result == DialogResult.Yes)
            {
                // Perform the deletion logic here
                InitSubSystem(TodoCommand.DELETE);

                await SubsystemPool.RunAllAsync(new CancellationToken());

                await RefreshTodos();

                MessageBox.Show("Item deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // User chose not to delete the item
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void todosList_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if(listBox.Items.Count == 0)
            {
                return;
            }

            TodoItem todo = listBox.Items[e.Index] as TodoItem;

            // Set the default background and foreground colors
            e.DrawBackground();
            Brush textBrush = SystemBrushes.ControlText;

            if(DateTime.Now > todo.ReminderDate && !todo.IsCompleted && !todo.IsNotifiedReminderDate)
            {
                e.Graphics.FillRectangle(Brushes.Cyan, e.Bounds);
            }
            if (DateTime.Now > todo.DueDate && !todo.IsCompleted && !todo.IsNotifiedDueDate)
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                textBrush = new SolidBrush(Color.White);
            }

            // Draw the text
            e.Graphics.DrawString(todo.ToString(), e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);

            // Draw focus rectangle if the ListBox item has focus
            e.DrawFocusRectangle();
        }
    }
}
