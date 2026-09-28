using System;
using System.Collections.Generic;
using System.Diagnostics;
using TaskManager.Models;

namespace TaskManager.Services
{
    internal class ProcessMonitor
    {
        public List<BlockedProcessInfo> FindBlockedApplications(FocusSession session)
        {
            List<BlockedProcessInfo> foundApps = new List<BlockedProcessInfo>();
            Dictionary<string, List<Process>> foundPaths = new Dictionary<string, List<Process>>(StringComparer.OrdinalIgnoreCase);

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    string processPath = process.MainModule.FileName;

                    if (process.ProcessName.ToLower().Contains("chrome"))
                    {
                        System.Diagnostics.Debug.WriteLine("chrome path: " + processPath);
                    }

                    SessionAppEntry entry;
                    if (session.Applications.TryGetValue(processPath, out entry) && entry.IsBlocked)
                    {
                        if (!foundPaths.ContainsKey(processPath))
                        {
                            foundPaths[processPath] = new List<Process>();
                        }
                        foundPaths[processPath].Add(process);
                    }
                    else
                    {
                        process.Dispose();
                    }
                }
                catch(Exception ex)
                {
                    if (process.ProcessName.ToLower().Contains("chrome"))
                    {
                        System.Diagnostics.Debug.WriteLine("chrome: " + ex.Message);
                    }
                }

            }

            foreach (KeyValuePair<string, List<Process>> pair in foundPaths)
            {
                string path = pair.Key;
                List<Process> processes = pair.Value;

                SessionAppEntry entry = session.Applications[path];

                foundApps.Add(new BlockedProcessInfo(processes, entry.Application));
            }
            return foundApps;
        }

        public void CloseApplication(List<Process> process)
        {
            try
            {
                foreach (var oneProcess in process)
                {
                    oneProcess.Kill();
                }
            }
            catch
            {
            }
        }
    }
}