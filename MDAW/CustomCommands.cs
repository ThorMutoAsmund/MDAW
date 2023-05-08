using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MDAW
{
    internal static class CustomCommands
    {
        //ApplicationCommands
        public static ICommand NewProject = new CustomCommand(FileCommands.NewProject);
        public static ICommand OpenProject = new CustomCommand(FileCommands.OpenProject);
        public static ICommand CloseProject = new CustomCommand(FileCommands.CloseProject, () => Env.Project != null);
        public static ICommand StartEditor = new CustomCommand(ApplicationCommands.StartEditor, () => Env.Project != null);
        public static ICommand Settings = new CustomCommand(ApplicationCommands.OpenSettings);
        public static ICommand ClearRecentFiles = new CustomCommand(Env.ClearRecentFiles);
        public static ICommand RecentFile1 = new CustomCommand(Env.OpenRecentFile1);
        public static ICommand ExitApplication = new CustomCommand(ApplicationCommands.ExitApplication);
        
        public static ICommand StopPlay = new CustomCommand(Audio.StopPlay, () => Env.Project != null);
        public static ICommand PlayPattern = new CustomCommand(Audio.PlayPattern, () => Env.Project != null);
        public static ICommand PlayFromStart = new CustomCommand(Audio.PlayFromStart, () => Env.Project != null);
        public static ICommand Stop = new CustomCommand(Audio.Stop, () => Env.Project != null);

        public static ICommand Record = new CustomCommand(ImportCommands.Record, () => Env.Project != null);
        public static ICommand RecordMidi = new CustomCommand(ImportCommands.RecordMidi, () => Env.Project != null);
        public static ICommand ImportFromYouTube = new CustomCommand(ImportCommands.ImportFromYouTube, () => Env.Project != null);
        public static ICommand DeleteSample = new CustomCommandWithParameter<string>(ImportCommands.DeleteSample, () => Env.Project != null);


        public static ICommand About = new CustomCommand(HelpCommands.About);
    }

    public class CustomCommandWithParameter<T> : ICommand where T : class
    {
#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        private Action<T> execute;
        private Func<bool>? canExecute;
        public virtual bool CanExecute(object? parameter) => this.canExecute?.Invoke() ?? true;
        public virtual void Execute(object? parameter)
        {
            if (parameter is T stringParameter)
            {
                this.execute?.Invoke(stringParameter);
            }
        }

        public CustomCommandWithParameter(Action<T> command, Func<bool>? canExecute = null)
        {
            this.execute = command;
            this.canExecute = canExecute;

            Env.ProjectChanged += OnCanExecuteChanged;
        }

        ~CustomCommandWithParameter()
        {
            Env.ProjectChanged -= OnCanExecuteChanged;
        }

        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CustomCommand : ICommand
    {
#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        private Action execute;
        private Func<bool>? canExecute;
        public virtual bool CanExecute(object? parameter) => this.canExecute?.Invoke() ?? true;
        public virtual void Execute(object? parameter)
        {
            this.execute();
        }

        public CustomCommand(Action command, Func<bool>? canExecute = null)
        {
            this.execute = command;
            this.canExecute = canExecute;

            Env.ProjectChanged += OnCanExecuteChanged;
        }

        ~CustomCommand()
        {
            Env.ProjectChanged -= OnCanExecuteChanged;
        }

        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
