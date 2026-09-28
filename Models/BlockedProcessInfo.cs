using System.Collections.Generic;
using System.Diagnostics;
namespace TaskManager.Models
{
    internal class BlockedProcessInfo
    {
        public List<Process> Process { get; }
        public InstalledApp Applications { get; }

        public BlockedProcessInfo(List<Process> process, InstalledApp installedApp)
        {
            Process = process;
            Applications = installedApp;
        }
    }
}
