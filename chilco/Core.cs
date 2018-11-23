using System;
using System.IO;
using System.Threading;

namespace chilco
{
    class Core
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Settings.Load();
            Settings.LoadProcessManagerProperties();
            string[] exePaths = Settings.ProcessManagerProperties[0, 1].Split(',');
            Console.WriteLine(Settings.ProcessManagerProperties[0, 1]);
            long time = Convert.ToInt64(Settings.ProcessManagerProperties[0, 2]);
            string name = Settings.ProcessManagerProperties[0, 0];
            ProcessManager.LogsPath = Settings.properties[1];
            ProcessManager pm = new ProcessManager(exePaths, time, name);
            while (true)
            {
                Thread.Sleep(1000);
                pm.Update();
            }
            Console.ReadKey();
        }
    }
}