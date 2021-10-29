using System;
using System.Collections.Generic;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants;
        public Queue<Track> Tracks = new Queue<Track>();

        public event EventHandler CompetitionFinished;

        public Track NextTrack()
        {
            try
            {
                return Tracks.Dequeue();
            }
            catch (InvalidOperationException)
            {
                CompetitionFinished?.Invoke(this, EventArgs.Empty);
                return null;
            }
        }
    }
}