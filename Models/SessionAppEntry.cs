using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    internal class SessionAppEntry
    {
        public InstalledApp Application { get; private set; }
        public bool IsBlocked { get; private set; }
        public SessionAppEntry(InstalledApp application, bool isBlocked)
        {
            Application = application;
            IsBlocked = isBlocked;
        }

        public void SetBlocked(bool isBlocked)
        {
            IsBlocked = isBlocked;
        }
    }
}
