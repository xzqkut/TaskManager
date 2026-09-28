using System;
using System.Collections.Generic;

namespace TaskManager.Models
{
    internal class FocusSession
    {
        private List<UserTask> _tasks;
        public FocusSession(TimeSpan timeSession)
        {

            if (timeSession <= TimeSpan.Zero)
            {
                throw new ArgumentException("Введите время целым числом больше нуля.");
            }

            _tasks = new List<UserTask>();
            Applications = new Dictionary<string, SessionAppEntry>(StringComparer.OrdinalIgnoreCase);
            TimeSession = timeSession;
        }

        public TimeSpan TimeSession { get; private set; }
        public Dictionary<string, SessionAppEntry> Applications { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public bool IsRunning { get; private set; }

        public bool IsFinished { get; private set; }

        public event Action SessionFinished;
        public void AddTaskToSession(UserTask task)
        {
            _tasks.Add(task);

        }

        public IReadOnlyList<UserTask> GetTasks()
        {
            return _tasks;
        }

        public void StartSession()
        {
            if (IsRunning || IsFinished)
            {
                return;
            }

            StartedAt = DateTime.Now;
            IsRunning = true;
        }

        public TimeSpan GetRemainingTime()
        {
            if (IsFinished)
            {
                return TimeSpan.Zero;
            }

            if (!IsRunning || StartedAt == null)
            {
                return TimeSession;
            }

            TimeSpan elapsedTime = DateTime.Now - StartedAt.Value;
            TimeSpan remainingTime = TimeSession - elapsedTime;

            if (remainingTime < TimeSpan.Zero)
            {
                return TimeSpan.Zero;
            }

            return remainingTime;
        }

        public void FinishSession()
        {
            IsRunning = false;
            IsFinished = true;
            SessionFinished?.Invoke();
        }
    }
}
