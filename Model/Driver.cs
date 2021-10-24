using System;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver(string name, IEquipment equipment, TeamColors teamColor)
        {
            Name = name;
            Equipment = equipment;
            TeamColor = teamColor;
        }

        public int GetMovementSpeed()
        {
            return Equipment.Performance * Equipment.Speed;
        }

        public bool WillBreak()
        {
            return new Random().Next(1, 100) == 69;
        }

        public bool WillRecover()
        {
            return new Random().Next(1, 10) == 7;
        }
    }
}