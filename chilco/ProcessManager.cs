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
            this.MaxPlaytime = (long) TimeConvert.MinutesToMillis((double) MaxPlaytime);
            this.ProcessGroupName = ProcessGroupName;
            if (File.Exists(LogsPath + ProcessGroupName + ".txt")) LoadLeftoverTime();
            else SaveLeftoverTime();
            if (LeftoverTime > 0) Enable();
        }

        #endregion Konstruktor

        public void Update()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("RUnning " + ProcessGroupName);
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
                        ProcessTime.Stop();
                        Disable();
                    }
                }
                else
                {
                    ProcessTime.Stop();
                }
                SaveLeftoverTime();
            }
        }

        /// <summary>
        /// puts a .disabled after the filename all .exe's, making them impossible to run
        /// ex. : firefox.exe --> firefox.exe.disabled
        /// </summary>
        public void Disable()
        {
            this.IsDisabled = true;
            //foreach (string ExePath in ExePaths)
                //File.Move(ExePath, ExePath + ".disabled");
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
                    //File.Move(ExePaths + ".disabled", ExePath);
                    this.IsDisabled = false;
            }
        }

        /// <summary>
        /// Saves the time that is left over to a file.
        /// </summary>
        public void SaveLeftoverTime()
        {
            string fileName = LogsPath + ProcessGroupName + ".txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(DateTime.Today.ToShortDateString());
                sw.WriteLine((LeftoverTime - ProcessTime.ElapsedMilliseconds).ToString());
            }
        }

        /// <summary>
        /// Loads Leftover Time from last session, adds MaxPlaytime to LeftoverTime for each day spent without Playing.
        /// </summary>
        public void LoadLeftoverTime()
        {
            try
            {
                string[] file = File.ReadAllLines(LogsPath + ProcessGroupName + ".txt");
                string[] date = file[0].Split('.');
                DateTime LastStartup = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));
                TimeSpan TimeSinceLastStartup = DateTime.Today - LastStartup;
                int DaysSinceLastStartup = TimeSinceLastStartup.Days;
                Console.WriteLine("days " + DaysSinceLastStartup);
                LeftoverTime = (DaysSinceLastStartup) * MaxPlaytime + Int32.Parse(file[1]);
            }
            //HACK If the Program fucks up the kid cant play for the rest of the day (Leftovertime = 0)
            catch(ArgumentOutOfRangeException) //If The date isnt in a correct date format.
            {
                SaveLeftoverTime();
                LoadLeftoverTime();
            }
            //HACK If the Program fucks up the kid cant play for the rest of the day (Leftovertime = 0)
            catch (FormatException)
            {
                SaveLeftoverTime();
                LoadLeftoverTime();
            }
        }

        /// <summary>
        /// checks if the processes are currently running
        /// </summary>
        /// <returns></returns>
        public bool ExeIsRunning(string ExePath) => Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ExePath)).Length > 0;

        public bool ExeAreRunning()
        {
            foreach (string ExePath in ExePaths)
            {
                if (ExeIsRunning(ExePath))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// kills a process.
        /// </summary>
        /// <param name="ExePath">Path to the .exe of the process to be killed</param>
        private void KillProcess(string ExePath)
        {
            foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ExePath)))
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