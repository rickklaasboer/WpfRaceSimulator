using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class DriverTests
    {
        private Driver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new Driver("Test", new Car(1, 1, 1, false), TeamColors.Red);
            _driver.Points = 1;
        }

        [Test]
        public void Driver_GetMovementSpeed_GetsMovementSpeed()
        {
            Assert.Greater(_driver.GetMovementSpeed(), 0);
        }


        [Test]
        public void Driver_WillBreak_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => { _driver.WillBreak(); });
        }


        [Test]
        public void Driver_WillRecover_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => { _driver.WillRecover(); });
        }

        [Test]
        public void Driver_Name_IsNotNull()
        {
            Assert.IsNotNull(_driver.Name);
        }

        [Test]
        public void Driver_Points_IsNotNull()
        {
            Assert.IsNotNull(_driver.Points);
        }

        [Test]
        public void Driver_Equipment_IsNotNull()
        {
            Assert.IsNotNull(_driver.Equipment);
            Assert.IsInstanceOf<IEquipment>(_driver.Equipment);
        }

        [Test]
        public void Driver_TeamColor_IsNotNull()
        {
            Assert.IsNotNull(_driver.TeamColor);
            Assert.IsInstanceOf<TeamColors>(_driver.TeamColor);
        }
    }
}