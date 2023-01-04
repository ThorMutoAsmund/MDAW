using MDAW.Properties;
using MDAWLib1;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDAW
{
    public static class HelpCommands
    {
        public static void About()
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            MessageBox.Show($"{Env.ApplicationName} version {version}\nBy Thor Muto Asmund (C) 2023", Env.ApplicationName);
        }
    }
}
