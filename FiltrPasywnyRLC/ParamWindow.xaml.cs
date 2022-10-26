using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Win32;
//using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;
using System.Data;

namespace FiltrPasywnyRLC
{
    /// <summary>
    /// Interaction logic for ParamWindow.xaml
    /// </summary>
    public partial class ParamWindow : Window
    {
        
        public ParamWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;    //Wlasciwosc DialogResult okna dialogowego ustawiana jest na True
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;   //Wlasciwosc DialogResult okna dialogowego ustawiana jest na False
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ExportToImage(parameters);
        }

        public static void ExportToImage(Grid canvas)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.DefaultExt = "png";
            dlg.FilterIndex = 2;
            dlg.FileName = "FilterParameters.png";
            dlg.RestoreDirectory = true;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            string path = dlg.FileName;
            int selectedFilterIndex = dlg.FilterIndex;
            if (result == true)
            {
                try
                {
                    double Height, renderHeight, Width, renderWidth;
                    Height = renderHeight = canvas.RenderSize.Height;
                    Width = renderWidth = canvas.RenderSize.Width;

                    RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
                    // needed otherwise the image output is black
                    canvas.Measure(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight));
                    canvas.Arrange(new Rect(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight)));

                    renderBitmap.Render(canvas);
                    BitmapEncoder imageEncoder = new PngBitmapEncoder();

                    if (selectedFilterIndex == 0) {}
                    else if (selectedFilterIndex == 1)
                    {
                        imageEncoder = new JpegBitmapEncoder();
                    }
                    else if (selectedFilterIndex == 2)
                    {
                        imageEncoder = new PngBitmapEncoder();
                    }
                    else if (selectedFilterIndex == 3)
                    {
                        imageEncoder = new JpegBitmapEncoder();
                    }
                    else if (selectedFilterIndex == 4)
                    {
                        imageEncoder = new GifBitmapEncoder();
                    }

                    imageEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                    using (FileStream file = File.Create(path))
                    {
                        imageEncoder.Save(file);

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

    }
}
