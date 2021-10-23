using System.Threading;
using Controller;

namespace racebaan
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRaceEvent += Visualize.OnNextRaceEvent;
            Data.NextRace();

            for (;;) Thread.Sleep(100);
        }
    }
}