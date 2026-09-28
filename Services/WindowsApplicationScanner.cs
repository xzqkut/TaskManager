using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using TaskManager.Models;

namespace TaskManager.Services
{
    internal class WindowsApplicationScanner
    {
        private const string UninstallKeyPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public List<InstalledApp> GetInstalledApps()
        {
            List<InstalledApp> apps = new List<InstalledApp>();

            HashSet<string> knownPaths =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            ReadApplications(
                Registry.LocalMachine.OpenSubKey(UninstallKeyPath),
                apps,
                knownPaths);

            ReadApplications(
                Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"),
                apps,
                knownPaths);

            ReadApplications(
                Registry.CurrentUser.OpenSubKey(UninstallKeyPath),
                apps,
                knownPaths);

            return apps
                .OrderBy(app => app.Name)
                .ToList();
        }

        private void ReadApplications(
            RegistryKey uninstallKey,
            List<InstalledApp> apps,
            HashSet<string> knownPaths)
        {
            if (uninstallKey == null)
            {
                return;
            }

            using (uninstallKey)
            {
                foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                {
                    using (RegistryKey appKey = uninstallKey.OpenSubKey(subKeyName))
                    {
                        if (appKey == null)
                        {
                            continue;
                        }

                        string name = appKey.GetValue("DisplayName") as string;
                        string displayIcon = appKey.GetValue("DisplayIcon") as string;

                        string exePath = ExtractExecutablePath(displayIcon);

                        if (string.IsNullOrWhiteSpace(name) ||
                            string.IsNullOrWhiteSpace(exePath) ||
                            !File.Exists(exePath) ||
                            !knownPaths.Add(exePath))
                        {
                            continue;
                        }

                        apps.Add(new InstalledApp(exePath, name));
                    }
                }
            }
        }

        private string ExtractExecutablePath(string displayIcon)
        {
            if (string.IsNullOrWhiteSpace(displayIcon))
            {
                return null;
            }

            string path = displayIcon.Trim();

            int commaIndex = path.LastIndexOf(',');

            if (commaIndex >= 0)
            {
                path = path.Substring(0, commaIndex);
            }

            path = path.Trim().Trim('"');

            if (!string.Equals(
                    Path.GetExtension(path),
                    ".exe",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return path;
        }
    }
}