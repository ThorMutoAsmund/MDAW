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
    public static class FileCommands
    {
        private static void Close()
        {
            if (Env.Project != null)
            {
                Env.Project = null;
                Env.HasChanges = false;
            }
        }

        public static void NewProject()
        {
            if (Dialogs.ConfirmChangesMade())
            {
                MessageBox.Show($"{nameof(NewProject)} not implemented yet", Env.ApplicationName);
            }
        }

        public static void OpenProject()
        {
            if (Dialogs.ConfirmChangesMade())
            {
                FileCommands.Close();

                if (Dialogs.SelectProject(out string selectedProjectPath))
                {
                    OpenProject(selectedProjectPath);
                }
            }
        }

        public static void OpenProject(string? projectPath)
        {
            if (projectPath != null && File.Exists(projectPath) && Project.TryLoadFromFile(projectPath, out var newProject))
            {
                Env.Project = newProject;
                Env.HasChanges = false;

                if (newProject != null)
                {
                    newProject.ReloadProjectDLL();
                    newProject.WatchForChanges();
                    Env.AddRecentFile(projectPath);
                }
            }
        }

        public static void CloseProject()
        {
            if (Dialogs.ConfirmChangesMade())
            {
                FileCommands.Close();
            }
        }
    }
}
