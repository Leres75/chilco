using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace chilco
{
    internal class ProcessManager
    {
        #region Attributes
        private bool IsDisabled { get; set; } = false;

        private string ProcessGroupName;
        private string[] ExePaths;
        public static string LogsPath { get; set; }
        private long MaxPlaytime { get; set; } = (long)TimeConvert.MinutesToMillis(30); //!!converting double to long!!
        public long LeftoverTime= 0;
        public Stopwatch ProcessTime = new Stopwatch();

        #endregion Attributes

        #region Konstruktor

        public ProcessManager(string[] ExePath)
        {
            this.ExePaths = ExePath;
            if (File.Exists(LogsPath + ProcessGroupName + ".txt")) LoadLeftoverTime();
            else SaveLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Milliseconds</param>
        public ProcessManager(string[] ExePath, long MaxPlaytime, string ProcessGroupName)
        {
            Console.WriteLine("long");
            this.ExePaths = ExePath;
            this.MaxPlaytime = MaxPlaytime;
            this.ProcessGroupName = ProcessGroupName;
            if (File.Exists(LogsPath + ProcessGroupName + ".txt")) LoadLeftoverTime();
            else SaveLeftoverTime();
            if (LeftoverTime > 0) Enable();
            Console.WriteLine(this.MaxPlaytime);
            Console.WriteLine(this.LeftoverTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExePath">Path to the exe</param>
        /// <param name="MaxPlaytime">Maxmimum Playtime in Minutes</param>
        public ProcessManager(string[] ExePath, int MaxPlaytime, string ProcessGroupName)
        {
            Console.WriteLine("int");
            this.ExePaths = ExePath;
            this.MaxPlaytime = TimeConvert.MinToMillis(MaxPlaytime);
            this.ProcessGroupName = ProcessGroupName;
            if (File.Exists(LogsPath + ProcessGroupName + ".txt")) LoadLeftoverTime();
            else SaveLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        #endregion Konstruktor

        public void Update()
        {
            if (ProcessTime.IsRunning) SaveLeftoverTime();
            if (ExeAreRunning())
            {
                Console.WriteLine("true");
                ProcessTime.Start();
                Console.WriteLine(ProcessTime.ElapsedMilliseconds);
                Console.WriteLine(LeftoverTime);
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
            SaveLeftoverTime();
        }

        /// <summary>
        /// puts a .disabled after the filename all .exe's, making them impossible to run
        /// ex. : firefox.exe --> firefox.exe.disabled
        /// </summary>
        public void Disable()
        {
            this.IsDisabled = true;
            foreach (string ExePath in ExePaths)
                File.Move(ExePath, ExePath + ".disabled");
        }

        /// <summary>
        /// removes the .disabled after all filenames.
        /// ex. : firefox.exe.disabled --> firefox.exe
        /// </summary>
        public void Enable()
        {
            if (this.IsDisabled)
            {
            foreach (string ExePath in ExePaths)
                if (File.Exists(ExePath + ".disabled"))
                    File.Move(ExePaths + ".disabled", ExePath);
                    this.IsDisabled = false;
            }
        }

        /// <summary>
        /// Saves the time that is left over to a file.
        /// </summary>
        public void SaveLeftoverTime()
        {
            if (!File.Exists(LogsPath + ProcessGroupName + ".txt"))
                File.Create(LogsPath + ProcessGroupName + ".txt");
            string[] output = new string[2];
            output[0] = DateTime.Today.ToShortDateString();
            output[1] = (LeftoverTime - ProcessTime.ElapsedMilliseconds) + "";
            File.WriteAllLines(LogsPath + ProcessGroupName + ".txt", output);
        }

        /// <summary>
        /// Loads Leftover Time from last session, adds MaxPlaytime to LeftoverTime for each day spent without Playing.
        /// </summary>
        public void LoadLeftoverTime()
        {
            string[] file = File.ReadAllLines(LogsPath + ProcessGroupName + ".txt");
            string[] date = file[0].Split('.');
            DateTime LastStartup = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));
            TimeSpan TimeSinceLastStartup = DateTime.Today - LastStartup;
            int DaysSinceLastStartup = TimeSinceLastStartup.Days;
            Console.WriteLine("days " + DaysSinceLastStartup);
            LeftoverTime = (DaysSinceLastStartup) * MaxPlaytime + Int32.Parse(file[1]);
        }

        /// <summary>
        /// checks if the processes are currently running
        /// </summary>
        /// <returns></returns>
        public bool ExeIsRunning(string ExePath) => Process.GetProcessesByName(Path.GetFileName(ExePath)).Length > 0;

        public bool ExeAreRunning()
        {
            foreach (string ExePath in ExePaths)
                if (ExeIsRunning(ExePath))
                    return true;
            return false;
        }

        /// <summary>
        /// kills a process.
        /// </summary>
        /// <param name="ExePath">Path to the .exe of the process to be killed</param>
        private void KillProcess(string ExePath)
        {
            foreach (Process process in Process.GetProcessesByName(Path.GetFileName(ExePath)))
            {
                if (ExeIsRunning(ExePath))
                {
                    process.Kill();
                }
            }
        }

        private void AddBonusTime(int amount) {
            if(amount > 0)
            LeftoverTime += amount;
        }

        private void CutDownTime(int amount)
        {
            if (amount < 0)
            LeftoverTime -= amount;
        }

    }
}