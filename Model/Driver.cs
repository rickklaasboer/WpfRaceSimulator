using System;
namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver()
        {
            
        }

        public Driver(string name)
        {
            Name = name;
        }
    }
}
