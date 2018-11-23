using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace chilco
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Settings.Load();
            Settings.LoadProcessManagerProperties();
            ProcessManager.LogsPath = Settings.properties[1];
            ProcessManager[] processManagers = new ProcessManager[Settings.ProcessManagerProperties.GetLength(0)];
            for (int i = 0; i < Settings.ProcessManagerProperties.GetLength(0); i++)
            {
                string[] exePaths = Settings.ProcessManagerProperties[i, 1].Split(',');
                long time = Convert.ToInt64(Settings.ProcessManagerProperties[i, 2]);
                string name = Settings.ProcessManagerProperties[i, 0];
                processManagers[i] = new ProcessManager(exePaths, time, name);
            }
            foreach (ProcessManager processManager in processManagers)
            {
                Thread t = new Thread(processManager.Update);
                t.Start();
            }
            //Console.ReadKey();
        }
    }
}
