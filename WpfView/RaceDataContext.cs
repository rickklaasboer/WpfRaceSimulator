using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Controller;
using Model;

namespace WpfView
{
    public class RaceDataContext : INotifyPropertyChanged
    {
        private List<IParticipant> _participants { get; set; }
        private Dictionary<IParticipant, int> _drivenLaps { get; set; }
        private Dictionary<IParticipant, int> _finished { get; set; }

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

        public Dictionary<IParticipant, int> DrivenLaps
        {
            get => _drivenLaps;
            set
            {
                _drivenLaps = value;
                OnPropertyChanged(nameof(DrivenLaps));
            }
        }

        public Dictionary<IParticipant, int> Finished
        {
            get => _finished;
            set
            {
                _finished = value;
                OnPropertyChanged(nameof(Finished));
            }
        }

        private void UpdateData(Race race)
        {
            Participants = new List<IParticipant>(race.Participants);
            DrivenLaps = new Dictionary<IParticipant, int>(race.DrivenLaps);
            Finished = new Dictionary<IParticipant, int>(race.Finished);
        }

        public void OnNextRaceEvent(object sender, NextRaceEventArgs args)
        {
            UpdateData(args.Race);
            OnPropertyChanged();
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            UpdateData((Race)sender);
            OnPropertyChanged();
        }

        private void OnPropertyChanged([AllowNull] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}