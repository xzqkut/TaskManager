using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для FocusTimerPage.xaml
    /// </summary>
    public partial class FocusTimerPage : Page
    {
        private DispatcherTimer _timenow;
        private FocusSession _focusSession;
        private ProcessMonitor _processMonitor;
        internal FocusTimerPage(FocusSession session)
        {
            InitializeComponent();
            _focusSession = session;
            _focusSession.SessionFinished += OnSessionFinished;
            _processMonitor = new ProcessMonitor();

            TaskTitleTextBlock1.Text = _focusSession.GetTasks().FirstOrDefault()?.Title ?? "Нет задач";
            RemainingTimeTextBlock.Text = _focusSession.GetRemainingTime().ToString(@"mm\:ss");

            _timenow = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timenow.Tick += TimerTick;
            _timenow.Start();


        }
        private void WarnAndCloseBlockedApplications()
        {
            List<BlockedProcessInfo> blockedApps =
                _processMonitor.FindBlockedApplications(_focusSession);

            foreach (BlockedProcessInfo app in blockedApps)
            {
                _processMonitor.CloseApplication(app.Process);
                MessageBox.Show(
                    "Запрещённое приложение будет закрыто: " + app.Applications.Name,
                    "FocusTask");

            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = _focusSession.GetRemainingTime();
            RemainingTimeTextBlock.Text =
                remainingTime.ToString(@"mm\:ss");
            WarnAndCloseBlockedApplications();
            if (remainingTime == TimeSpan.Zero)
            {
                _focusSession.FinishSession();
            }
        }

        public void OnSessionFinished()
        {
            _timenow.Stop();
            SessionFinishedWindow finishedWindow = new SessionFinishedWindow();
            finishedWindow.ShowDialog();
        }
    }
}
