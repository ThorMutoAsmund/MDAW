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
            ApplySettings();

            AddMessage($"======= {Env.ApplicationName} Started =======");
            SetTitle();

            if (Env.IsDebug)
            {
                Env.LastProjectPath = @"C:\Repos\MDAW\DemoSong";
            }

            Env.AddMessage += AddMessage;
            Env.ProjectChanged += SetTitle;
            Env.HasChangesChanged += SetTitle;
        }

        private void SetTitle()
        {
            this.Title = Env.Project != null ?
                $"{Env.ProjectName}{(Env.HasChanges ? "*" : string.Empty)} - {Env.ApplicationName}" :
                Env.ApplicationName;
        }

        #region Messages
        private void AddMessage(string s)
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
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Dialogs.ConfirmChangesMade();
        }
    }
}
