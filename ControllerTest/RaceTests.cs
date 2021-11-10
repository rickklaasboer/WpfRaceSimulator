using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    /**
     * The Race class is really difficult to test because everything runs on a timer.
     * This is the best I can do ¯\_(ツ)_/¯
     */
    [TestFixture]
    public class RaceTests
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
            var sections = new[]
            {
                SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Finish
            };

            var track = new Track("TestTrack", sections);

            var participants = new List<IParticipant>
            {
                new Driver("TestDriver1", new Car(1, 1, 25, false), TeamColors.Red),
                new Driver("TestDriver2", new Car(1, 1, 25, false), TeamColors.Green),
                new Driver("TestDriver3", new Car(1, 1, 25, false), TeamColors.Blue),
                new Driver("TestDriver4", new Car(1, 1, 25, false), TeamColors.Yellow),
            };

            _race = new Race(track, participants, 1);
        }

        [Test]
        public void Race_GetSectionData_IsNotNull()
        {
            var track = _race.Track.Sections.First?.Value;
            Assert.IsNotNull(_race.GetSectionData(track));
        }

        [Test]
        public void Race_Start_ExecutesWithoutErrors()
        {
            Assert.DoesNotThrow(() =>
            {
                _race.Start();
                _race.CleanUp();
            });
        }

        [Test]
        public void Race_CleanUp_ExecutesWithoutErrors()
        {
            Assert.DoesNotThrow(() => { _race.CleanUp(); });
        }
    }
}