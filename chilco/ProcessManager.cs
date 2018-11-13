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
            File.WriteAllText(LogsPath + Path.GetFileName(ExePath) + ".txt",DateTime.Today.ToShortDateString() + "\n" + (MaxPlaytime + LeftoverTime - ProcessTime.ElapsedMilliseconds));
        }

        public void LoadLeftoverTime()
        {
            string[] file = File.ReadAllLines(LogsPath + Path.GetFileName(ExePath) + ".txt");
            string[] date = file[0].Split('.');
            DateTime LastStartup = new DateTime(Int32.Parse(date[3]), Int32.Parse(date[2]), Int32.Parse(date[1]));
            TimeSpan TimeSinceLastStartup = DateTime.Today - LastStartup;
            int DaysSinceLastStartup = TimeSinceLastStartup.Days;
            LeftoverTime = DaysSinceLastStartup * MaxPlaytime + Int32.Parse(file[1]);
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
