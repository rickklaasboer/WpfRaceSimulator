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
        public Dictionary<IParticipant, int> DrivenLaps = new Dictionary<IParticipant, int>();
        public Dictionary<IParticipant, int> Finished = new Dictionary<IParticipant, int>();
        public int Laps;
        public List<IParticipant> Participants;

        private DateTime _startTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();
        private Timer _timer = new Timer(250);

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler<EventArgs> RaceFinished;
        public event EventHandler<ParticipantFinishedEventArgs> ParticipantFinished;
        public event EventHandler<FinishReachedEventArgs> FinishReached;

        public Race(Track track, List<IParticipant> participant, int laps)
        {
            Track = track;
            Participants = participant;

            Laps = laps;
            _random = new Random(DateTime.Now.Millisecond);

            // Event handlers
            _timer.Elapsed += OnTimedEvent;
            FinishReached += OnFinishReached;
            ParticipantFinished += OnParticipantFinished;
            RaceFinished += OnRaceFinished;

            DetermineStartingPositions();
            RandomizeEquipment();
        }

        public void Start()
        {
            _startTime = DateTime.Now;
            _timer.Start();
        }

        public void CleanUp()
        {
            DriversChanged = null;
            RaceFinished = null;
            ParticipantFinished = null;
            FinishReached = null;
            _timer.Stop();
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
                RemoveFinishedParticipants(sectionData);
                MoveSingleParticipant(Side.Left, sectionData, section);
                MoveSingleParticipant(Side.Right, sectionData, section);
            }
        }

        private void MoveSingleParticipant(Side side, SectionData sectionData, Section section)
        {
            (IParticipant participant, int distance) = sectionData.GetDataBySide(side);
            (IParticipant nextParticipant, int _) = GetSectionData(Track.GetNextSection(section)).GetDataBySide(side);

            if (participant == null) return;

            var newDistance = distance + participant.GetMovementSpeed();

            if (!participant.Equipment.IsBroken && nextParticipant == null)
            {
                if (newDistance >= 100)
                {
                    var nextSection = Track.GetNextSection(section);
                    var nextSectionData = GetSectionData(nextSection);

                    if (nextSection.SectionType == SectionTypes.Finish)
                    {
                        FinishReached?.Invoke(this, new FinishReachedEventArgs(participant));
                    }

                    nextSectionData.SetDataBySide(side, participant, 0);
                    sectionData.SetDataBySide(side, null, 0);
                }
                else
                {
                    sectionData.SetDataBySide(side, participant, newDistance);
                }
            }
            else if (newDistance >= 100 && nextParticipant != null)
            {
                var nextSection = Track.GetNextSection(section);
                var nextNextSection = Track.GetNextSection(nextSection);

                var nextNextSectionData = GetSectionData(nextNextSection);

                if (nextSection.SectionType == SectionTypes.Finish)
                {
                    FinishReached?.Invoke(this, new FinishReachedEventArgs(participant));
                }

                nextNextSectionData.SetDataBySide(side, participant, 0);
                sectionData.SetDataBySide(side, null, 0);
            }
            else
            {
                sectionData.SetDataBySide(side, participant, newDistance);
            }
        }

        private void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(1, 10);
                participant.Equipment.Performance = _random.Next(1, 10);
            }
        }

        private void DetermineStartingPositions()
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

        private void RemoveFinishedParticipants(SectionData sectionData)
        {
            if (sectionData?.Left != null && Finished.ContainsKey(sectionData.Left))
            {
                sectionData.Left = null;
            }

            if (sectionData?.Right != null && Finished.ContainsKey(sectionData.Right))
            {
                sectionData.Right = null;
            }
        }

        private void BreakParticipants()
        {
            foreach (var participant in Participants)
            {
                if (!participant.Equipment.IsBroken)
                {
                    participant.Equipment.IsBroken = participant.WillBreak();
                }
                else
                {
                    participant.Equipment.IsBroken = !participant.WillRecover();
                }
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs args)
        {
            BreakParticipants();
            MoveParticipants();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
        }

        private void OnFinishReached(object sender, FinishReachedEventArgs args)
        {
            var prevLaps = DrivenLaps.GetValueOrDefault(args.Participant, -1);

            if (prevLaps != -1)
            {
                DrivenLaps[args.Participant]++;

                if (DrivenLaps[args.Participant] >= Laps)
                {
                    ParticipantFinished?.Invoke(this, new ParticipantFinishedEventArgs(args.Participant));
                }
            }
            else
            {
                DrivenLaps[args.Participant] = 0;
            }
        }

        private void OnParticipantFinished(object sender, ParticipantFinishedEventArgs args)
        {
            var participant = args.Participant;

            List<int> points = (Enumerable.Range(1, Participants.Count + 1).Reverse()).ToList();

            int value = Finished.Count + 1;
            Finished[participant] = value;
            participant.Points += points[value];

            if (Finished.Count >= Participants.Count)
            {
                RaceFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnRaceFinished(object sender, EventArgs args)
        {
            Data.NextRace();
        }
    }
}