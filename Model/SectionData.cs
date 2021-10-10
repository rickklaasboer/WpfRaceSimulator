namespace Model
{
    public class SectionData
    {
        public IParticipant Left;
        public int DistanceLeft;
        public IParticipant Right;
        public int DistanceRight;

        public SectionData() {}
        
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
    }
}
