using MDAWLib1;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDAW
{
    public static class ApplicationCommands
    {
        public static void OpenSettings()
        {
            SettingsWindows.Open();
        }
        
        public static void StartEditor()
        {
            MessageBox.Show($"{nameof(StartEditor)} not implemented yet");
        }

        public static void ExitApplication()
        {
            if (Dialogs.ConfirmChangesMade())
            {
                Environment.Exit(0);
            }

        }
    }
}
