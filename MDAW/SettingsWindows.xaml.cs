using MDAW.Properties;
using System.Windows;

namespace MDAW
{
    /// <summary>
    /// Interaction logic for SettingsWindows.xaml
    /// </summary>
    public partial class SettingsWindows : Window
    {
        public SettingsWindows()
        {
            InitializeComponent();
            
            this.Topmost = Settings.Default.StayOnTop;
        }

        public static bool Open()
        {
            var window = new SettingsWindows();
            return window.ShowDialog() == true;
        }
    }
}
