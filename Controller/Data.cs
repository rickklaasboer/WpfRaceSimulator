using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition;
        public static Race CurrentRace;

        public static void Initialize()
        {
            Competition = new Competition();

            AddParticipants();
            AddTracks();
        }

        private static void AddParticipants()
        {
            Competition.Participants = new List<IParticipant>
            {
                new Driver("Pieter", new Car(1, 1, 25, false)),
                new Driver("Henk", new Car(1, 1, 25, false)),
                new Driver("Max", new Car(1, 1, 25, false)),
                new Driver("Tjeerd", new Car(1, 1, 25, false))
            };
        }

        private static void AddTracks()
        {
            Competition.Tracks = new Queue<Track>(new[]
            {
                new Track("Track 1", new[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight
                })
            });
        }

        public static void NextRace()
        {
            Track track = Competition.NextTrack();

            if (track != null)
            {
                CurrentRace = new Race(track, Competition.Participants, 1);
            }
        }
    }
}