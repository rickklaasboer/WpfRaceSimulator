using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Controller;
using Model;

namespace WpfView
{
    public class CompetitionDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IParticipant> Participants { get; set; }

        public IParticipant BestParticipant { get; set; }
        
        public IParticipant FastestParticipant { get; set; }

        public void OnNextRaceEvent(object sender, NextRaceEventArgs args)
        {
            Participants = args.Race.Participants;
            BestParticipant = Participants.OrderByDescending(p => p.Points).First();
            FastestParticipant = Participants.OrderByDescending(p => p.Equipment.Speed).First();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}