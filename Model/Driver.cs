using System;
namespace Model
{
    public class Driver : IParticipant
    {
        string IParticipant.Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IParticipant.Points { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IEquipment IParticipant.Equipment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TeamColors IParticipant.TeamColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
