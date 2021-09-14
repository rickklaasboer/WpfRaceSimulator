using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public SectionData GetSectionData(Section section) {
            return _positions.GetValueOrDefault(section);
        }
    }
}
