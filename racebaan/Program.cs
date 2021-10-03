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
            Visualize.Initialize();

            Data.NextRace();
            Visualize.DrawTrack(Data.CurrentRace.Track);

            while (true) Thread.Sleep(100);
        }
    }
}