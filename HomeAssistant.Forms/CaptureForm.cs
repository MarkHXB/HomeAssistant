using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HomeAssistant.Forms
{
    public partial class CaptureForm : Form
    {
        private readonly Bitmap screenshot;
        private readonly Rectangle selectionRectangle;
        private readonly string outputPath;

        public CaptureForm(Bitmap screenshot, Rectangle selectionRectangle, string outputPath)
        {
            InitializeComponent();
            this.screenshot = screenshot;
            this.selectionRectangle = selectionRectangle;
            this.outputPath = outputPath;
        }

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            // Save the selected region to the specified output path
            SaveSelectedRegion();
            this.Close(); // Close the form after saving the screenshot
        }

        private void SaveSelectedRegion()
        {
            // Create a new bitmap containing only the selected region
            using (Bitmap selectedRegionBitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height))
            {
                using (Graphics g = Graphics.FromImage(selectedRegionBitmap))
                {
                    g.DrawImage(screenshot, 0, 0, selectionRectangle, GraphicsUnit.Pixel);
                }

                // Save the selected region to the specified output path
                selectedRegionBitmap.Save(outputPath, ImageFormat.Png);
            }      
        }
    }
}
