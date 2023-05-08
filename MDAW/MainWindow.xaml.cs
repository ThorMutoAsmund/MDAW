using MDAW.Properties;
using MDAWLib1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Env.MainWindow = this;

            ApplySettings();

            Env_AddMessage($"======= {Env.ApplicationName} Started =======");
            SetTitle();

            if (Env.IsDebug)
            {
                Env.LastProjectPath = @"C:\Repos\MDAW\DemoSong";
            }

            Env.AddMessage += Env_AddMessage;
            Env.ProjectChanged += Env_ProjectChanged;
            Env.DLLReloaded += SetTitle;
            Env.HasChangesChanged += SetTitle;
            Env.RecentFilesChanged += Env_RecentFilesChanged;

            PlaybackContext.RenderFinished += RenderFinished;
        }

        private void Env_ProjectChanged()
        {
            SetTitle();
            Audio.Stop();
        }

        private void SetTitle()
        {
            this.Title = Env.Project != null ?
                $"{Env.ApplicationName} - {Env.ProjectSongTitle}{(Env.HasChanges ? "*" : string.Empty)} - {Env.ProjectRootPath}" :
                Env.ApplicationName;
        }

        #region Messages
        private void Env_AddMessage(string s)
        {
            this.outputTextBlock.Inlines.Add($"{(this.outputTextBlock.Text == string.Empty ? "" : "\n")}{s}");
            this.outputScrollViewer.ScrollToBottom();
        }

        private void outputTextBlockClear_Click(object sender, RoutedEventArgs e)
        {
            this.outputTextBlock.Inlines.Clear();
        }
        #endregion

        #region Settings
        private void StayOnTopMenu_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.StayOnTop = this.stayOnTopMenu.IsChecked;
            Settings.Default.Save();
            ApplySettings();
        }

        private void ApplySettings()
        {
            this.stayOnTopMenu.IsChecked = Settings.Default.StayOnTop;
            this.Topmost = Settings.Default.StayOnTop;

            Env_RecentFilesChanged();
        }

        private void Env_RecentFilesChanged()
        {
            this.recentFilesMenu.IsEnabled = Settings.Default.RecentFiles != null && Settings.Default.RecentFiles.Count > 0;
            this.recentFilesMenu.Items.Clear();

            if (Settings.Default.RecentFiles != null)
            {
                foreach (var recentFile in Env.RecentFiles)
                {
                    var subMenu = new MenuItem()
                    {
                        Header = recentFile
                    };
                    subMenu.Click += (object sender, RoutedEventArgs e) =>
                    {
                        var file = (sender as MenuItem)?.Header as string;
                        FileCommands.OpenProject(file);
                    };

                    this.recentFilesMenu.Items.Add(subMenu);
                }
            }
        }

        private void RenderFinished(double seconds)
        {
            Env.OnAddMessage($"Rendered {seconds} seconds of audio");
        }

        private void SubMenu_Click(object sender, RoutedEventArgs e)
        {
            var file = (sender as MenuItem)?.Header;
            Debug.WriteLine(file);
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Dialogs.ConfirmChangesMade();
        }
    }
}
