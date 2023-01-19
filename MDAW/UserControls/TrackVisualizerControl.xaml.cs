using MDAWLib1;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TrackVisualizerControl.xaml
    /// </summary>
    public partial class TrackVisualizerControl : UserControl
    {
        public TrackVisualizerControl()
        {
            InitializeComponent();

            Env.DLLReloaded += Env_DLLReloaded;
            Env.ProjectChanged += Env_ProjectChanged;
        }

        private void Env_ProjectChanged()
        {
            this.mainStackPanel.Children.Clear();
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

            if (project.Song?.SampleProvider is IHasInputs hasInputs)
            {
                foreach (var track in hasInputs.Inputs)
                {
                    var trackControl = new TrackControl()
                    {
                        Height = 64
                    };
                    this.mainStackPanel.Children.Add(trackControl);

                    if (track is IVisualTrack visualTrack)
                    {
                    }
                }
            }
        }
    }
}
