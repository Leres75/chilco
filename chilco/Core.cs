using System;
using System.Collections.Generic;

namespace chilco
{
    internal class Core
    {
            Console.WriteLine("Hello World!");
            Settings.Load();
            Console.ReadKey();
            Settings.LoadProcessManagerProperties();
        List<SingleProcessManager> programs = new List<SingleProcessManager>();
    }
}