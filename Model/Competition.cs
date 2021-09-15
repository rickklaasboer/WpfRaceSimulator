using System;
using System.Collections.Generic;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants;
        public Queue<Track> Tracks;

        public Track NextTrack()
        {
            try
            {
                return Tracks.Dequeue();
            } catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
