using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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
        private Timer _timer = new Timer(500);

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public Race(Track track, List<IParticipant> participant)
        {
            Track = track;
            Participants = participant;
            _random = new Random(DateTime.Now.Millisecond);

            _timer.Elapsed += OnTimedEvent;

            DetermineStartingPositions();
            RandomizeEquipment();
        }

        public void Start()
        {
            _timer.Start();
        }


        public SectionData GetSectionData(Section section)
        {
            if (_positions.ContainsKey(section))
            {
                return _positions.GetValueOrDefault(section);
            }

            var sectionData = new SectionData();
            _positions.Add(section, sectionData);

            return sectionData;
        }

        private void MoveParticipants()
        {
            foreach (var (section, sectionData) in new Dictionary<Section, SectionData>(_positions).Reverse())
            {
                if (sectionData.Left != null)
                {
                    var newDistanceLeft = sectionData.DistanceLeft +
                                          (sectionData.Left.Equipment.Performance * sectionData.Left.Equipment.Speed);

                    if (newDistanceLeft >= 100)
                    {
                        var nextSectionData = GetSectionData(Track.GetNextSection(section));
                        nextSectionData.Left = sectionData.Left;
                        nextSectionData.DistanceLeft = 0;

                        sectionData.Left = null;
                        sectionData.DistanceLeft = 0;
                    }
                    else
                    {
                        sectionData.DistanceLeft += newDistanceLeft;
                    }
                }

                if (sectionData.Right != null)
                {
                    var newDistanceRight = sectionData.DistanceRight +
                                           (sectionData.Right.Equipment.Performance *
                                            sectionData.Right.Equipment.Speed);

                    if (newDistanceRight >= 100)
                    {
                        var nextSectionData = GetSectionData(Track.GetNextSection(section));
                        nextSectionData.Right = sectionData.Right;
                        nextSectionData.DistanceRight = 0;

                        sectionData.Right = null;
                        sectionData.DistanceRight = 0;
                    }
                    else
                    {
                        sectionData.DistanceRight += newDistanceRight;
                    }
                }
            }
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 3);
                participant.Equipment.Performance = _random.Next(1, 3);
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
                        sectionData.DistanceLeft = 25;
                    }

                    if (index < Participants.Count && sectionData.Right == null)
                    {
                        sectionData.Right = Participants[index++];
                        sectionData.DistanceRight = 25;
                    }
                }
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs args)
        {
            MoveParticipants();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
        }
    }
}