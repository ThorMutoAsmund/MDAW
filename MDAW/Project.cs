using MDAWLib1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MDAW
{
    public class Project
    {
        public string ProjectPath { get; private set; }
        public string RootPath { get; private set; }
        public string ProjectName { get; private set; }
        public string Target { get; private set; }
        public ProjectConfiguration Configuration { get; private set; }
        public string DLLName { get; private set; }
        public string DLLPath { get; private set; }
        public string MainClassName { get; private set; }

        public Assembly? Assembly { get; private set; }
        public Song? Song { get; private set; }

        private Project(string projectPath, string target, ProjectConfiguration configuration)
        {
            this.ProjectPath = projectPath;
            this.Target = target;
            this.Configuration = configuration;
            this.RootPath = Path.GetDirectoryName(this.ProjectPath) ?? string.Empty;
            this.ProjectName = Path.GetFileNameWithoutExtension(this.ProjectPath);
            this.DLLName = Path.ChangeExtension(Path.GetFileName(this.ProjectPath), ".dll");
            this.MainClassName = $"{this.ProjectName}.Main";
            this.DLLPath = Path.Combine(this.RootPath, "bin", this.Configuration.ToString("g"), this.Target, this.DLLName);
        }

        public static bool TryLoadFromFile(string projectPath, out Project? project)
        {
            project = null;

            if (File.Exists(projectPath))
            {
                XElement booksFromFile = XElement.Load(projectPath);

                XName propertyGroupName = XName.Get("PropertyGroup");
                XName targetFrameworkName = XName.Get("TargetFramework");

                var propertyGroupNode = booksFromFile.Element(propertyGroupName);
                if (propertyGroupNode == null)
                {
                    Dialogs.Error("No PropertyGroup defined");
                    return false;
                }

                var targetFrameworkNode = propertyGroupNode.Element(targetFrameworkName);
                if (targetFrameworkNode == null)
                {
                    Dialogs.Error("No TargetFramework set");
                    return false;
                }

                var projectTarget = targetFrameworkNode.Value;

                if (OpenProjectWindow.Open(projectPath, projectTarget, out var configuration))
                {
                    project = new Project(projectPath, projectTarget, configuration);

                    return true;
                }
            }

            return false;
        }

        public void ReloadProjectDLL()
        {
            try
            {
                var alc = new AssemblyLoadContext("MDAWPlugin", true);

                using (var fs = File.Open(this.DLLPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    this.Assembly = alc.LoadFromStream(fs);

                    if (this.Assembly != null)
                    {
                        var type = this.Assembly.GetType(this.MainClassName);
                        if (type != null)
                        {
                            var myObject = Activator.CreateInstance(type);
                            if (myObject != null)
                            {
                                this.Song = myObject as Song;
                                if (this.Song != null)
                                {
                                    Env.OnAddMessage($"Reloaded '{this.Song.Title}'");
                                    
                                    PlaybackContext.CreateFromSong(this.Song);
                                }
                                else
                                {
                                    Env.OnAddMessage($"{this.MainClassName} not of type Song");
                                }
                            }
                            else
                            {
                                Env.OnAddMessage($"Could not create instance of {this.MainClassName}");
                            }
                        }
                        else
                        {
                            Env.OnAddMessage($"Could not create type {this.MainClassName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Env.OnAddMessage(ex.Message);
                return;
            }
        }

    }
}
