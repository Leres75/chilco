using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace chilco
{
    class ProcessManager
    {
        public string ExePath;
        public string LogPath;
        public Stopwatch ProcessTime = new Stopwatch();
        public ProcessManager(string ExePath)
        {
            this.ExePath = ExePath;
        }

        public ProcessManager(string ExePath, string LogPath)
        {
            this.ExePath = ExePath;
            this.LogPath = LogPath;
        }

        public void Disable()
        {
            System.IO.File.Move(ExePath, ExePath + ".disabled");
        }

        public bool IsRunning()
        {
            if (Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0)
            {
                if (!ProcessTime.IsRunning) ProcessTime.Start();
                return true;
            }
            else
            {
                if (ProcessTime.IsRunning) ProcessTime.Stop();
            }
            return false;
        }
    }
}
