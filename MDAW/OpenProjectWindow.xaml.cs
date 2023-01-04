using MDAWLib1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MDAW
{
    /// <summary>
    /// Interaction logic for OpenProjectWindow.xaml
    /// </summary>
    public partial class OpenProjectWindow : Window
    {
        public OpenProjectWindow()
        {
            InitializeComponent();

            this.configurationComboBox.ItemsSource = Enum.GetValues(typeof(ProjectConfiguration));
            this.configurationComboBox.SelectedIndex = 0;
        }

        public static bool Open(string projectPath, string projectTarget, out ProjectConfiguration configuration)
        {
            var window = new OpenProjectWindow();

            window.projectPathTextBox.Text = projectPath;
            window.projectTargetTextBox.Text = projectTarget;

            var result = window.ShowDialog() == true;

            configuration = (ProjectConfiguration)window.configurationComboBox.SelectedValue;

            return result;
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
