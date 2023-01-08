using MDAW.Properties;
using MDAWLib1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace MDAW
{
    public static class Env
    {
        public static event Action<string>? AddMessage;
        public static event Action? HasChangesChanged;
        public static event Action? ProjectChanged;
        public static event Action? RecentFilesChanged;

        public static string ApplicationName = "MDAW";
        public static bool IsDebug => true;
        public static bool HasChanges
        {
            get => hasChanges;
            set
            {
                if (value != hasChanges)
                {
                    if (value && Project == null)
                    {
                        return;
                    }

                    hasChanges = value;
                    HasChangesChanged?.Invoke();
                }
            }
        }

        public static Project? Project
        {
            get => project;
            set
            {
                if (value != project)
                {
                    project = value;
                    ProjectChanged?.Invoke();
                }
            }
        }

        public static IEnumerable<string> RecentFiles
        {
            get => Settings.Default.RecentFiles?.Cast<string>().ToList() ?? new List<string>();
        }

        public static void AddRecentFile(string recentFile)
        {
            if (Settings.Default.RecentFiles == null)
            {
                Settings.Default.RecentFiles = new System.Collections.Specialized.StringCollection();
            }
            Settings.Default.RecentFiles.Add(recentFile);
            Settings.Default.Save();

            RecentFilesChanged?.Invoke();
        }

        public static void ClearRecentFiles()
        {
            Settings.Default.RecentFiles = null;
            Settings.Default.Save();

            RecentFilesChanged?.Invoke();
        }

        public static void OpenRecentFile1()
        {
            if (Env.RecentFiles != null && Env.RecentFiles?.Count() > 0)
            {
                FileCommands.OpenProject(Env.RecentFiles.ElementAt(0));
            }
        }

        public static string ProjectName => Project != null ? Path.GetFileNameWithoutExtension(Project.ProjectPath) : string.Empty;

        public static string LastProjectPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        private static bool hasChanges;
        private static Project? project;
        public static void OnAddMessage(string message)
        {
            AddMessage?.Invoke(message);
        }

    }
}
