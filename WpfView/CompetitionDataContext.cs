using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Controller;
using Model;

namespace WpfView
{
    public class CompetitionDataContext : INotifyPropertyChanged
    {
        private List<IParticipant> _participants { get; set; }
        private IParticipant _bestParticipant { get; set; }
        private IParticipant _fastestParticipant { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public List<IParticipant> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                OnPropertyChanged(nameof(Participants));
            }
        }

        public IParticipant BestParticipant
        {
            get => _bestParticipant;
            set
            {
                {
                    _bestParticipant = value;
                    OnPropertyChanged(nameof(BestParticipant));
                }
            }
        }

        public IParticipant FastestParticipant
        {
            get => _fastestParticipant;
            set
            {
                _fastestParticipant = value;
                OnPropertyChanged(nameof(FastestParticipant));
            }
        }

        private void UpdateData(List<IParticipant> participants)
        {
            Participants = new List<IParticipant>(participants);
            BestParticipant = Participants.OrderByDescending(p => p.Points).First();
            FastestParticipant = Participants.OrderByDescending(p => p.Equipment.Performance).First();
        }

        public void OnNextRaceEvent(object sender, NextRaceEventArgs args)
        {
            UpdateData(args.Race.Participants);
            OnPropertyChanged();
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            Race race = (Race)sender;
            UpdateData(race.Participants);
        }

        private void OnPropertyChanged([AllowNull] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}