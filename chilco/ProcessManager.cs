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
        public static string LogsPath;
        public long MaxPlaytime;
        public long LeftoverTime;
        public Stopwatch ProcessTime = new Stopwatch();
        public ProcessManager(string ExePath)
        {
            this.ExePath = ExePath;
        }
        
        public void Update()
        {
            if (ProcessTime.IsRunning) SaveLeftoverTime();
            if (ExeIsRunning())
            {
                ProcessTime.Start();
                if (ProcessTime.ElapsedMilliseconds > MaxPlaytime + LeftoverTime)
                {
                    Disable();
                }
            }
            else
            {
                ProcessTime.Stop();
            }
        }
        public void Disable()
        {
            File.Move(ExePath, ExePath + ".disabled");
        }

        public void SaveLeftoverTime()
        {
            File.WriteAllText(LogsPath + Path.GetFileName(ExePath) + ".txt", "" + (MaxPlaytime + LeftoverTime - ProcessTime.ElapsedMilliseconds));
        }

        public bool ExeIsRunning()
        {
            if (Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
