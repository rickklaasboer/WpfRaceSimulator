using System;
using Controller;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Data_NextRaceShould
    {
        [SetUp]
        public void SetUp()
        {
            //
        }

        [Test]
        public void Data_Competition_IsNotNull()
        {
            Data.Initialize();
            Assert.IsNotNull(Data.Competition);
        }

        [Test]
        public void Data_CurrentRace_IsNull()
        {
            Data.Initialize();
            Assert.IsNull(Data.CurrentRace);
        }

        [Test]
        public void Data_NextRace_ShouldCallEvent()
        {
            int calls = 0;
            
            Data.Initialize();
            Data.NextRaceEvent += (sender, args) => { calls++; };
            Data.NextRace();

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Data_NextRace_CurrentRace_IsNotNull()
        {
            Data.Initialize();
            Data.NextRace();
            
            Assert.IsNotNull(Data.CurrentRace);
        }
    }
}