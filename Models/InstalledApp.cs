using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    internal class InstalledApp
    {
        public InstalledApp(string path, string name)
        {
            Path = path;
            Name = name;
        }
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
