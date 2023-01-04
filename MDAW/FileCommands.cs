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
                    if (File.Exists(selectedProjectPath) && Project.TryLoadFromFile(selectedProjectPath, out var newProject))
                    {
                        Env.Project = newProject;
                        Env.HasChanges = false;

                        if (Env.Project != null)
                        {
                            Env.Project.ReloadProjectDLL();
                        }
                    }
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
