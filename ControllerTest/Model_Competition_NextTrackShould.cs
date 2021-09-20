using System.Collections.Generic;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("Track 1", null);
            
            _competition.Tracks = new Queue<Track>(new Track[]
            {
                track
            });

            var result = _competition.NextTrack();
            Assert.AreEqual(result, track);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("Track 1", null);
            
            _competition.Tracks = new Queue<Track>(new Track[]
            {
                track
            });

            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track firstTrack = new Track("Track 1", null);
            Track secondTrack = new Track("Track 2", null);
            
            _competition.Tracks = new Queue<Track>(new Track[]
            {
                firstTrack,
                secondTrack
            });

            var result1 = _competition.NextTrack();
            var result2 = _competition.NextTrack();
            
            Assert.AreEqual(firstTrack, result1);
            Assert.AreEqual(secondTrack, result2);
        }
    }
}