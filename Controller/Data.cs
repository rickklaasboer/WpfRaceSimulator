using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition;

        public static void Initialize()
        {
            Competition = new Competition();

            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants = new List<IParticipant> { new Driver(), new Driver(), new Driver() }; 
        }

        public static void AddTracks()
        {
            Competition.Tracks = new Queue<Track>(new Track[] { new Track("Track 1", null) });
        }
    }
}
