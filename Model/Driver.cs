namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver(string name, IEquipment equipment)
        {
            Name = name;
            Equipment = equipment;
        }

        public int GetMovementSpeed()
        {
            // TODO: make this variable
            return 25;
            // return Equipment.Performance * Equipment.Speed;
        }
    }
}