using MDAWLib1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDAW
{
    /// <summary>
    /// Interaction logic for TrackControl.xaml
    /// </summary>
    public partial class TrackControl : UserControl
    {
        private IVisualTrack visualTrack;
        private int startValue;
        private int zoomLevel = 1;
        private double scaleY = 4.0;
        private System.Drawing.Pen wavePen = new System.Drawing.Pen(System.Drawing.Color.Black);

        public TrackControl(IVisualTrack visualTrack)
        {
            InitializeComponent();

            this.visualTrack = visualTrack;
        }

        private void Render()
        {
            var halfHeight = mainCanvas.ActualHeight / 2;
            using (var bmp = new Bitmap((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    gfx.Clear(System.Drawing.Color.Beige);
                    var from = this.startValue;
                    var prevPoint = new System.Drawing.Point(0, (int)halfHeight);
                    for (int i = 0; i < mainCanvas.ActualWidth; i++)
                    {
                        var sampleFrom = (from + i) * this.zoomLevel;
                        var sampleTo = (from + i + 1) * this.zoomLevel;
                        if (sampleTo >= this.visualTrack.VisualBuffer.Length)
                        {
                            break;
                        }

                        var value = this.visualTrack.VisualBuffer[sampleFrom];
                        if (this.zoomLevel > 1)
                        {
                            for (var z = sampleFrom + 1; z < sampleTo; ++z)
                            {
                                value += this.visualTrack.VisualBuffer[z];
                            }
                            value /= this.zoomLevel;
                        }


                        double waveValue = value * this.scaleY * halfHeight;

                        var y = Math.Max(Math.Min(halfHeight - waveValue, mainCanvas.ActualHeight), 0);

                        if (i > 0)
                        {
                            var pt = new System.Drawing.Point(i, (int)y);
                            gfx.DrawLine(this.wavePen, prevPoint, pt);
                            prevPoint = pt;
                        }
                    }

                    mainImage.Source = BmpImageFromBmp(bmp);
                    mainImage.Width = mainCanvas.ActualWidth;
                    mainImage.Height = mainCanvas.ActualHeight;
                }
            }
        }

        private BitmapImage BmpImageFromBmp(Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public void SetStartValue(int startValue, int zoomLevel)
        {
            this.startValue = startValue;
            this.zoomLevel = zoomLevel;
            Render();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Render();
        }

        private void mainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //Render();
        }
    }
}
