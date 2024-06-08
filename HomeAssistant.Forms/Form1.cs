namespace HomeAssistant.Forms
{
    public partial class Form1 : Form
    {
        private Point startPoint;
        private Rectangle selectionRectangle;
        private Bitmap screenshot;
        private string saveOutputPath;

        public Form1(string saveOutputPath)
        {
            InitializeComponent();
            this.saveOutputPath = saveOutputPath;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Cross;
            this.Opacity = 0.01; // Set opacity to a very low value to make the form transparent
            //this.BackColor = Color.Transparent; // Make the form transparent
            //this.TransparencyKey = this.BackColor;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                this.MouseDown -= MainForm_MouseDown; // Unsubscribe from the MouseDown event
                this.MouseMove += MainForm_MouseMove; // Subscribe to the MouseMove event
                this.MouseUp += MainForm_MouseUp; // Subscribe to the MouseUp event
            }

            //startPoint = e.Location;
            //Invalidate();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point tempEndPoint = e.Location;
                selectionRectangle = new Rectangle(
                    Math.Min(startPoint.X, tempEndPoint.X),
                    Math.Min(startPoint.Y, tempEndPoint.Y),
                    Math.Abs(startPoint.X - tempEndPoint.X),
                    Math.Abs(startPoint.Y - tempEndPoint.Y));
                this.Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, selectionRectangle);
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                using (Graphics g = Graphics.FromImage(screenshot))
                {
                    g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                    g.DrawRectangle(Pens.Red, selectionRectangle);
                }

                // Do something with the captured screenshot
                // For example, display it in another form
                using (CaptureForm captureForm = new CaptureForm(screenshot, selectionRectangle, saveOutputPath))
                {
                    captureForm.ShowDialog();
                }

                // Set focus back to the main form
                this.Focus();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
