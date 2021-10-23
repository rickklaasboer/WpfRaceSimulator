using System;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {
        public readonly Track Track;

        public DriversChangedEventArgs(Track track)
        {
            Track = track;
        }
    }
}