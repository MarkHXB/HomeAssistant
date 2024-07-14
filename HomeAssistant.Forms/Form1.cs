using System.Runtime.InteropServices;

namespace HomeAssistant.Forms
{
    public partial class Form1 : Form
    {
        private Point startPoint;
        private Rectangle selectionRectangle;
        private Bitmap screenshot;
        private string saveOutputPath;

        // P/Invoke declarations
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int
        wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);

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
            using (Pen pen = new Pen(Color.Blue, 3))
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

                this.Focus();

                Size sz = Screen.PrimaryScreen.Bounds.Size;
                IntPtr hDesk = GetDesktopWindow();
                IntPtr hSrce = GetWindowDC(hDesk);
                IntPtr hDest = CreateCompatibleDC(hSrce);
                IntPtr hBmp = CreateCompatibleBitmap(hSrce, selectionRectangle.Width, selectionRectangle.Height);
                IntPtr hOldBmp = SelectObject(hDest, hBmp);
                bool b = BitBlt(hDest, 0, 0, selectionRectangle.Width, selectionRectangle.Height, hSrce, selectionRectangle.X, selectionRectangle.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
                Bitmap bmp = Bitmap.FromHbitmap(hBmp);
                SelectObject(hDest, hOldBmp);
                DeleteObject(hBmp);
                DeleteDC(hDest);
                ReleaseDC(hDesk, hSrce);
                bmp.Save(saveOutputPath);
                bmp.Dispose();

                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
