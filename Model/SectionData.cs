using System;
using System.ComponentModel;

namespace Model
{
    public enum Side
    {
        Right,
        Left
    }

    public class SectionData
    {
        public IParticipant Left;
        public int DistanceLeft;
        public IParticipant Right;
        public int DistanceRight;

        public SectionData()
        {
        }

        public SectionData(IParticipant participant, int distance, bool right)
        {
            if (right)
            {
                Right = participant;
                DistanceRight = distance;
            }
            else
            {
                Left = participant;
                DistanceRight = distance;
            }
        }

        public (IParticipant, int) GetDataBySide(Side side)
        {
            return side switch
            {
                Side.Left => (Left, DistanceLeft),
                Side.Right => (Right, DistanceRight),
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        public void SetDataBySide(Side side, IParticipant participant, int distance)
        {
            switch (side)
            {
                case Side.Left:
                    Left = participant;
                    DistanceLeft = distance;
                    break;
                case Side.Right:
                    Right = participant;
                    DistanceRight = distance;
                    break;
            }
        }
    }
}