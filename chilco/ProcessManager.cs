using System;
using System.Diagnostics;
using System.IO;

namespace chilco
{
    internal class ProcessManager
    {
        #region Attributes

        private string ProcessGroupName;
        private string[] ExePaths;
        public static string LogsPath { get; set; }
        private long MaxPlaytime { get; set; } = TimeConvert.MinToMillis(30);
        public long LeftoverTime;
        public Stopwatch ProcessTime = new Stopwatch();

        #endregion Attributes

        #region Konstruktor

        public ProcessManager(string[] ExePath)
        {
            this.ExePaths = ExePath;
            LoadLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Milliseconds</param>
        public ProcessManager(string[] ExePath, long MaxPlaytime, string ProcessGroupName)
        {
            this.ExePaths = ExePath;
            this.MaxPlaytime = MaxPlaytime;
            this.ProcessGroupName = ProcessGroupName;
            LoadLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Minutes</param>
        public ProcessManager(string[] ExePath, int MaxPlaytime, string ProcessGroupName)
        {
            this.ExePaths = ExePath;
            this.MaxPlaytime = TimeConvert.MinToMillis(MaxPlaytime);
            this.ProcessGroupName = ProcessGroupName;
            LoadLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        #endregion Konstruktor

        public void Update()
        {
            if (ProcessTime.IsRunning) SaveLeftoverTime();
            if (ExeAreRunning())
            {
                ProcessTime.Start();
                if (ProcessTime.ElapsedMilliseconds > LeftoverTime)
                {
                    foreach (string ExePath in ExePaths)
                        KillProcess(ExePath);
                    Disable();
                }
            }
            else
            {
                ProcessTime.Stop();
            }
        }

        /// <summary>
        /// puts a .disabled after the filename all .exe's, making them impossible to run
        /// ex. : firefox.exe --> firefox.exe.disabled
        /// </summary>
        public void Disable()
        {
            foreach (string ExePath in ExePaths)
                File.Move(ExePath, ExePath + ".disabled");
        }

        /// <summary>
        /// removes the .disabled after all filenames.
        /// ex. : firefox.exe.disabled --> firefox.exe
        /// </summary>
        public void Enable()
        {
            foreach (string ExePath in ExePaths)
                if (File.Exists(ExePath + ".disabled"))
                    File.Move(ExePaths + ".disabled", ExePath);
        }

        /// <summary>
        /// Saves the time that is left over to a file.
        /// </summary>
        public void SaveLeftoverTime()
        {
            File.WriteAllText(LogsPath + ProcessGroupName + ".txt", DateTime.Today.ToShortDateString() + "\n" + (LeftoverTime - ProcessTime.ElapsedMilliseconds));
        }

        /// <summary>
        /// Loads Leftover Time from last session, adds MaxPlaytime to LeftoverTime for each day spent without Playing.
        /// </summary>
        public void LoadLeftoverTime()
        {
            string[] file = File.ReadAllLines(LogsPath + ProcessGroupName + ".txt");
            string[] date = file[0].Split('.');
            DateTime LastStartup = new DateTime(Int32.Parse(date[3]), Int32.Parse(date[2]), Int32.Parse(date[1]));
            TimeSpan TimeSinceLastStartup = DateTime.Today - LastStartup;
            int DaysSinceLastStartup = TimeSinceLastStartup.Days;
            LeftoverTime = (DaysSinceLastStartup + 1) * MaxPlaytime + Int32.Parse(file[1]);
        }

        /// <summary>
        /// checks if the processes are currently running
        /// </summary>
        /// <returns></returns>
        public bool ExeAreRunning()
        {
            foreach (string ExePath in ExePaths)
                if (Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0)
                    return true;
            return false;
        }

        /// <summary>
        /// kills a process.
        /// </summary>
        /// <param name="ExePath">Path to the .exe of the process to be killed</param>
        private void KillProcess(string ExePath)
        {
            foreach (var process in Process.GetProcessesByName(Path.GetFileName(ExePath)))
            {
                if (Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0)
                    process.Kill();
            }
        }
    }
}