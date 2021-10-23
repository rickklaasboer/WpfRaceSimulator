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

            Visualize.Initialize();
            Visualize.DrawTrack(Data.CurrentRace.Track);

            Data.CurrentRace.Start();

            for (;;) Thread.Sleep(100);
        }
    }
}