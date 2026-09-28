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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;

namespace TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskSetupPage.xaml
    /// </summary>
    public partial class TaskSetupPage : Page
    {
        public TaskSetupPage()
        {
            InitializeComponent();

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();

            int minutes;

            if (!int.TryParse(DurationMinutesTextBox.Text, out minutes) || minutes <= 0)
            {
                MessageBox.Show("Введите время целым числом больше нуля.");
                return;
            }

            try
            {
                UserTask task = new UserTask(title, description);
                FocusSession session = new FocusSession(TimeSpan.FromMinutes(minutes));
                session.AddTaskToSession(task);
                NavigationService.Navigate(new AppSelectionPage(session));
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
