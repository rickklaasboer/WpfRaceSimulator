﻿using System;
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
        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();

        public Race(Track track, List<IParticipant> participant)
        {
            Track = track;
            Participants = participant;
            _random = new Random(DateTime.Now.Millisecond);
            
            DetermineStartingPositions();
        }

        public SectionData GetSectionData(Section section) {
            if (_positions.ContainsKey(section))
            {
                return _positions.GetValueOrDefault(section);
            }
            
            var sectionData = new SectionData();
            _positions.Add(section, sectionData);

            return sectionData;
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }

        public void DetermineStartingPositions()
        {
            var index = 0;
            foreach (var section in Track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {

                    var sectionData = GetSectionData(section);
                    if (index < Participants.Count && sectionData.Left == null)
                    {
                        sectionData.Left = Participants[index++];
                        sectionData.DistanceLeft = 0;
                    }

                    if (index < Participants.Count && sectionData.Right == null)
                    {
                        sectionData.Right = Participants[index++];
                        sectionData.DistanceRight = 10;
                    }
                }
            }
        }
    }
}
