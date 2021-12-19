using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class SectionDataTests
    {
        private SectionData _sectionData;

        private IParticipant _rightParticipant;
        private IParticipant _leftParticipant;

        [SetUp]
        public void SetUp()
        {
            _rightParticipant = new Driver("TestDriver1", new Car(1, 1, 25, false), TeamColors.Red);
            _leftParticipant = new Driver("TestDriver2", new Car(1, 1, 25, false), TeamColors.Green);

            _sectionData = new SectionData
            {
                Right = _rightParticipant,
                Left = _leftParticipant
            };
        }

        [Test]
        public void SectionData_GetDataBySide_GetsDataBySide()
        {
            (IParticipant rightParticipant, int rightDistance) = _sectionData.GetDataBySide(Side.Right);
            (IParticipant leftParticipant, int leftDistance) = _sectionData.GetDataBySide(Side.Left);

            Assert.IsInstanceOf<Driver>(rightParticipant);
            Assert.IsInstanceOf<Driver>(leftParticipant);

            Assert.GreaterOrEqual(rightDistance, 0);
            Assert.GreaterOrEqual(leftDistance, 0);
        }

        [Test]
        public void SectionData_SetDataBySide_SetsDataBySide()
        {
            _sectionData.SetDataBySide(Side.Left, null, 0);
            _sectionData.SetDataBySide(Side.Right, null, 0);

            Assert.IsNull(_sectionData.Left);
            Assert.IsNull(_sectionData.Right);

            Assert.AreEqual(0, _sectionData.DistanceLeft);
            Assert.AreEqual(0, _sectionData.DistanceRight);
        }
    }
}