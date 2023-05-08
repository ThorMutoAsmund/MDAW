using MDAW.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MDAW
{
    public class Watchers
    {
        public List<string> SamplesList { get; private set; } = new List<string>();

        private FileSystemWatcher? samplesWatcher;
        private string samplesPath;

        public Watchers(Project project)
        {
            this.samplesPath = Path.Combine(project.RootPath, Settings.Default.SamplesFolder);

            ConfigureWatchers();
            RescanSamples();
        }

        ~Watchers()
        { 
            StopWatchers();
        }

        private void ConfigureWatchers()
        {
            // Samples
            var context = SynchronizationContext.Current;
            if (context == null)
            {
                Env.OnAddMessage("Cannot watch samples folder. Getting synchronization context failed");
                return;
            }

            this.samplesWatcher = new FileSystemWatcher()
            {
                Path = samplesPath,
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.*",
            };
            this.samplesWatcher.Changed += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Created += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Deleted += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Renamed += (source, e) => context.Post(val => RescanSamples(), source);

            this.samplesWatcher.EnableRaisingEvents = true;

            //// Scripts
            //this.scriptsWatcher = new FileSystemWatcher()
            //{
            //    Path = Env.Song.ScriptsPath,
            //    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            //    Filter = "*.cs",
            //};
            //this.scriptsWatcher.Changed += (source, e) => context.Post(val => RescanSripts(true), source);
            //this.scriptsWatcher.Created += (source, e) => context.Post(val => RescanSripts(), source);
            //this.scriptsWatcher.Deleted += (source, e) => context.Post(val => RescanSripts(), source);
            //this.scriptsWatcher.Renamed += (source, e) => context.Post(val => RescanSripts(), source);

            //this.scriptsWatcher.EnableRaisingEvents = true;
        }

        private void StopWatchers()
        {
            if (this.samplesWatcher != null)
            {
                this.samplesWatcher.EnableRaisingEvents = false;

                this.SamplesList.Clear();
            }
        }

        private void RescanSamples()
        {
            try
            {
                this.SamplesList = Directory.EnumerateFiles(this.samplesPath, "*.*").Select(x => System.IO.Path.GetFileName(x)).ToList();
            }
            catch (Exception)
            {
                this.SamplesList.Clear();
            }

            Env.OnSamplesListChanged(this.SamplesList);
        } 
    }
}