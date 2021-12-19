using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition;
        public static Race CurrentRace;

        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

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
                new Driver("Finn", new Car(1, 1, 25, false), TeamColors.Blue),
                new Driver("James", new Car(1, 1, 25, false), TeamColors.Green),
                new Driver("Liam", new Car(1, 1, 25, false), TeamColors.Red),
                new Driver("Noah", new Car(1, 1, 25, false), TeamColors.Yellow)
            };
        }

        private static void AddTracks()
        {
            Competition.Tracks = new Queue<Track>(new[]
            {
                new Track("Track 1", new[]
                {
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.StartGrid,
                    SectionTypes.Finish, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner,
                    SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight
                }),
                new Track("Track 2", new[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.LeftCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner
                }),
                new Track("Track 3", new[]
                {
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.StartGrid,
                    SectionTypes.Finish, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.LeftCorner,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner
                })
            });
        }

        public static void NextRace()
        {
            CurrentRace?.CleanUp();

            Track track = Competition.NextTrack();

            if (track != null)
            {
                CurrentRace = new Race(track, Competition.Participants, 2);
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs(CurrentRace));
                CurrentRace.Start();
            }
        }
    }
}