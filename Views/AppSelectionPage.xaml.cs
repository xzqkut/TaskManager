using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Views
{
    public partial class AppSelectionPage : Page
    {
        private readonly FocusSession _session;

        internal AppSelectionPage(FocusSession session)
        {
            InitializeComponent();

            _session = session;

            TaskTitleTextBlock.Text = _session.GetTasks()[0].Title;

            LoadApplications();
        }

        private void LoadApplications()
        {
            WindowsApplicationScanner scanner =
                new WindowsApplicationScanner();

            List<InstalledApp> apps = scanner.GetInstalledApps();

            foreach (InstalledApp app in apps)
            {
                SessionAppEntry entry =
                    new SessionAppEntry(app, false);

                _session.Applications.Add(app.Path, entry);

                CheckBox checkBox = new CheckBox
                {
                    Content = app.Name,
                    Tag = entry,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                checkBox.Checked += ApplicationCheckBoxChanged;
                checkBox.Unchecked += ApplicationCheckBoxChanged;

                ApplicationsPanel.Children.Add(checkBox);
            }
        }

        private void ApplicationCheckBoxChanged(
            object sender,
            RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            SessionAppEntry entry =
                checkBox.Tag as SessionAppEntry;

            entry.SetBlocked(checkBox.IsChecked == true);
        }

        private void StartFocusButton_Click(object sender, RoutedEventArgs e)
        {
           _session.StartSession();
            NavigationService.Navigate(new FocusTimerPage(_session));
        }
    }
}