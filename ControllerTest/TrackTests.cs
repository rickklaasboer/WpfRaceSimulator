using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class TrackTests
    {
        private Track _track;

        [SetUp]
        public void SetUp()
        {
            _track = new Track("TestTrack", new[]
            {
                SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Finish
            });
        }

        [Test]
        public void Track_GetNextSection_GetsNextSection()
        {
            Assert.IsInstanceOf<Section>(_track.GetNextSection(_track.Sections.First?.Value));
        }

        [Test]
        public void Track_GetPreviousSection_GetsPreviousSection()
        {
            Assert.IsInstanceOf<Section>(_track.GetPreviousSection(_track.Sections.First?.Value));
        }
        
        [Test]
        public void Track_GetNextSection_OnLastSection_GetsNextSection()
        {
            Assert.IsInstanceOf<Section>(_track.GetNextSection(_track.Sections.Last?.Value));
        }

        [Test]
        public void Track_GetPreviousSection_OnFirstSection_GetsPreviousSection()
        {
            Assert.IsInstanceOf<Section>(_track.GetPreviousSection(_track.Sections.First?.Value));
        }
    }
}