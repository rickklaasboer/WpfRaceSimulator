using System;

namespace Controller
{
    public class NextRaceEventArgs : EventArgs
    {
        public Race Race;

        public NextRaceEventArgs(Race race)
        {
            Race = race;
        }
    }
}