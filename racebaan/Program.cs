using System;
using System.Threading;
using Controller;

namespace racebaan
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();

            Data.NextRace();

            Console.WriteLine($"{Data.CurrentRace.Track.Name}");

            while (true) Thread.Sleep(100);
        }
    }
}
