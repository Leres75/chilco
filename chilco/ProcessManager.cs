using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace chilco
{
    class ProcessManager
    {
        #region Attributes
        private string ExePath;
        public static string LogsPath { get; set; }
        private long MaxPlaytime { get; set; } = TimeConvert.MinToMillis(30);
        public long LeftoverTime;
        public Stopwatch ProcessTime = new Stopwatch();
        #endregion Attributes

        #region Konstruktor

        public ProcessManager(string ExePath)
        {
            this.ExePath = ExePath;
            LoadLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Milliseconds</param>
        public ProcessManager(string ExePath, long MaxPlaytime)
        {
            this.ExePath = ExePath;
            this.MaxPlaytime = MaxPlaytime;
            LoadLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Minutes</param>
        public ProcessManager(string ExePath, int MaxPlaytime)
        {
            this.ExePath = ExePath;
            this.MaxPlaytime = TimeConvert.MinToMillis(MaxPlaytime);
            LoadLeftoverTime();
        }

        #endregion Konstruktor

        public void Update()
        {
            if (ProcessTime.IsRunning) SaveLeftoverTime();
            if (ExeIsRunning())
            {
                ProcessTime.Start();
                if (ProcessTime.ElapsedMilliseconds > LeftoverTime)
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
        /// removes the .disabled after the filename.
        /// ex. : firefox.exe.disabled --> firefox.exe
        /// </summary>
        public void Enable()
        {
            if(File.Exists(ExePath + ".disabled"))
                File.Move(ExePath + ".disabled", ExePath);
        }

        /// <summary>
        /// Saves the time that is left over to a file.
        /// </summary>
        public void SaveLeftoverTime()
        {
            File.WriteAllText(LogsPath + Path.GetFileName(ExePath) + ".txt",DateTime.Today.ToShortDateString() + "\n" + (LeftoverTime - ProcessTime.ElapsedMilliseconds));
        }
        /// <summary>
        /// Loads Leftover Time from last session, adds MaxPlaytime to LeftoverTime for each day spent without Playing.
        /// </summary>
        public void LoadLeftoverTime()
        {
            string[] file = File.ReadAllLines(LogsPath + Path.GetFileName(ExePath) + ".txt");
            string[] date = file[0].Split('.');
            DateTime LastStartup = new DateTime(Int32.Parse(date[3]), Int32.Parse(date[2]), Int32.Parse(date[1]));
            TimeSpan TimeSinceLastStartup = DateTime.Today - LastStartup;
            int DaysSinceLastStartup = TimeSinceLastStartup.Days;
            LeftoverTime = (DaysSinceLastStartup + 1) * MaxPlaytime + Int32.Parse(file[1]);
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
