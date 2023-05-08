using MDAW.Properties;
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
    public static class ImportCommands
    {
        public static void Record()
        {
            if (Env.Project == null)
            {
                return;
            }

            var dialog = RecordDialog.Create();
            if (dialog.ShowDialog() == true)
            {
            }
        }

        public static void RecordMidi()
        {
            if (Env.Project == null)
            {
                return;
            }

            Dialogs.NotImplemented();
        }

        public static async void ImportFromYouTube()
        {
            if (Env.Project == null)
            {
                return;
            }

            var dialog = StringDialog.Create("Enter YouTube Video URL");
            
            if (dialog.ShowDialog() == true)
            {
                var path = Path.Combine(Env.Project.RootPath, Settings.Default.SamplesFolder);
                if (Dialogs.CreateFolderIfNotFound(path))
                {
                    await YouTubeImport.Import(dialog.Value, path);
                }
            }
        }

        public static void DeleteSample(string sampleName)
        {
            if (Env.Project == null)
            {
                return;
            }

            if (Dialogs.Confirm($"This wlil delete the sample {sampleName}"))
            {
                var filePath = Path.Combine(Env.Project.RootPath, Settings.Default.SamplesFolder, sampleName);

                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Dialogs.Message($"Error deleting file: {ex.Message}");
                }
            }
        }
    }
}
