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
                    KillProcess();
                    Disable();
                }
            }
            else
            {
                ProcessTime.Stop();
            }
        }

        /// <summary>
        /// puts a .disabled after the filename of the .exe, making it impossible to run
        /// ex. : firefox.exe --> firefox.exe.disabled
        /// </summary>
        public void Disable()
        {
            File.Move(ExePath, ExePath + ".disabled");
        }

        /// <summary>
        /// Saves the time that is left over to a file.
        /// </summary>
        public void SaveLeftoverTime()
        {
            File.WriteAllText(LogsPath + Path.GetFileName(ExePath) + ".txt", "" + (MaxPlaytime + LeftoverTime - ProcessTime.ElapsedMilliseconds));
        }

        /// <summary>
        /// checks if the process is currently running
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// kills the process.
        /// </summary>
        private void KillProcess()
        {
            foreach (var process in Process.GetProcessesByName(Path.GetFileName(ExePath)))
            {
                if (Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0)
                    process.Kill();
            }
        }
    }
}
