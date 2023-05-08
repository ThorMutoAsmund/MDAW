using MDAW.Properties;
using MDAWLib1;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MDAW
{
    /// <summary>
    /// Interaction logic for SamplesControl.xaml
    /// </summary>
    public partial class SamplesControl : UserControl
    {

        public SamplesControl()
        {
            InitializeComponent();

            Env.ProjectChanged += Env_ProjectChanged;
            Env.SamplesListChanged += Env_SamplesListChanged;
        }

        private void Env_ProjectChanged()
        {
            if (Env.Project == null)
            {
                this.DataContext = null;
            }
        }

        private void Env_SamplesListChanged(List<string> sampleList)
        {
            this.DataContext = sampleList;
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Env.Project == null)
            {
                return;
            }

            if (e.ChangedButton == MouseButton.Left && ((FrameworkElement)e.OriginalSource).DataContext is string sampleName)
            {
                var samplePath = Path.Combine(Env.Project.RootPath, Settings.Default.SamplesFolder, sampleName);

                Audio.PlayFile(samplePath);
            }
        }

        private void listView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Audio.Stop();
        }

        //private void importMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    //Samples.ImportSamples();
        //}

        private void deleteMenu_Click(object sender, RoutedEventArgs e)
        {
            //Samples.DeleteSamples(this.listView.SelectedItems.Cast<string>());
        }
    }
}
