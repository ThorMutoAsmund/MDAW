using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDAW
{
    public static class Dialogs
    {
        public static string WaveFilesFilter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*";
        public static string ProjectFilesFilter = "Project files (*.csproj)|*.csproj";
        public static bool ConfirmChangesMade()
        {
            if (Env.HasChanges)
            {
                if (MessageBox.Show("Changes have been made. Continue without saving?", "Changes Made", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool SelectProject(out string selectedProjectPath)
        {
            var result = SelectFile("Open Project", Env.LastProjectPath, out selectedProjectPath, ProjectFilesFilter);

            if (result)
            {
                var newPath = Path.GetDirectoryName(selectedProjectPath);
                if (newPath != null)
                {
                    Env.LastProjectPath = newPath;
                }
            }

            return result;
        }
            

        public static bool SelectFile(string description, string initialPath, out string selectedFile, string filter)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = description;
                dialog.InitialDirectory = initialPath;
                dialog.Filter = filter;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                selectedFile = dialog.FileName;

                return result == System.Windows.Forms.DialogResult.OK;
            }
        }

        public static bool SelectMultipleFiles(string description, string initialPath, out string[] selectedFiles, string filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*")
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = description;
                dialog.InitialDirectory = initialPath;
                dialog.Filter = filter;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                selectedFiles = dialog.FileNames;

                return result == System.Windows.Forms.DialogResult.OK;
            }
        }

        //public static bool BrowseFiles(string description, string initialPath, out string[] selectedFiles, string filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*")
        //{
        //    using (var dialog = new System.Windows.Forms.OpenFileDialog())
        //    {
        //        dialog.Multiselect = true;
        //        dialog.Title = description;
        //        dialog.InitialDirectory = initialPath;
        //        dialog.Filter = filter;
        //        System.Windows.Forms.DialogResult result = dialog.ShowDialog();

        //        selectedFiles = dialog.FileNames;

        //        return result == System.Windows.Forms.DialogResult.OK;
        //    }
        //}

        public static void Message(string message)
        {
            MessageBox.Show(message, Env.ApplicationName);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, Env.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
