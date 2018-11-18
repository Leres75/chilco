using System;

namespace chilco
{
    class Core
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Settings.Load();
            Settings.LoadProcessManagerProperties();
            Console.ReadKey();
        }
    }
}
