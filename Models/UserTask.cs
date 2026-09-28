using System;

namespace TaskManager.Models
{
    internal class UserTask
    {
        public UserTask(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Название задачи не может быть пустым.");
            }
            Title = title;
            Description = description;
        }

        public string Description { get; private set; }
        public string Title { get; private set; }

    }
}
