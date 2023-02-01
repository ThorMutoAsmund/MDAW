using MDAWLib1;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MDAW
{
    /// <summary>
    /// Interaction logic for TrackVisualizerControl.xaml
    /// </summary>
    public partial class TrackVisualizerControl : UserControl
    {
        private int maxLength;
        private int zoomLevel = 1;
        private int previousZoomLevel = 1;

        public TrackVisualizerControl()
        {
            InitializeComponent();

            Env.DLLReloaded += Env_DLLReloaded;
            Env.ProjectChanged += Env_ProjectChanged;
            Audio.PositionChanged += Audio_PositionChanged;

            HideScrollBars();
        }

        ~TrackVisualizerControl()
        {
            Audio.PositionChanged -= Audio_PositionChanged;
        }

        private void Audio_PositionChanged(TimeSpan position)
        {
            this.Dispatcher.Invoke(() => {
                this.positionTextBlock.Text = position.ToString("mm\\:ss\\.ff");
            });
        }

        private void Env_ProjectChanged()
        {
            this.mainStackPanel.Children.Clear();
            HideScrollBars();
        }

        private void HideScrollBars()
        { 
            this.mainScrollBar.Minimum = 0;
            this.mainScrollBar.Maximum = 0;
            this.zoomScrollBar.Minimum = 0;
            this.zoomScrollBar.Maximum = 0;
        }

        private void Env_DLLReloaded()
        {
            if (Env.Project != null)
            {
                RedrawProject(Env.Project);
            }
        }

        private void RedrawProject(Project project)
        {
            this.mainStackPanel.Children.Clear();

            if (project.Song?.Provider is IHasInputs hasInputs)
            {
                int maxLength = 0;
                foreach (var track in hasInputs.Inputs)
                {
                    if (track is IVisualTrack visualTrack)
                    {
                        var trackControl = new TrackControl(visualTrack)
                        {
                            Height = 64
                        };

                        this.mainStackPanel.Children.Add(trackControl);

                        this.maxLength = Math.Max(maxLength, visualTrack.VisualBuffer.Length);
                    }
                }

                UpdateScrollBars();
            }
        }

        private void UpdateScrollBars()
        {
            this.zoomScrollBar.Minimum = 0;
            this.zoomScrollBar.Maximum = 19;
            var newValue = this.mainScrollBar.Value * this.previousZoomLevel / this.zoomLevel;
            this.mainScrollBar.Minimum = 0;
            this.mainScrollBar.Maximum = this.maxLength / this.zoomLevel;
            this.mainScrollBar.ViewportSize = this.mainScrollBar.Maximum / 20;
            this.mainScrollBar.Value = newValue;

            this.previousZoomLevel = this.zoomLevel;
        }

        private void mainScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RepaintTracks();
            Debug.WriteLine("Repainting");
        }

        private void RepaintTracks()
        { 
            var position = (int)this.mainScrollBar.Value;
            foreach (var child in this.mainStackPanel.Children)
            {
                (child as TrackControl)?.SetStartValue(position, this.zoomLevel);
            }
        }

        private void zoomScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var value = (this.zoomScrollBar.Value / 2f + 1);
            this.zoomLevel = (int)(value * value);
            UpdateScrollBars();
            RepaintTracks();
        }
    }
}
