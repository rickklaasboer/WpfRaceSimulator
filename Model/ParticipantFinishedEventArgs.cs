using System;

namespace Model
{
    public class ParticipantFinishedEventArgs : EventArgs
    {
        public IParticipant Participant;

        public ParticipantFinishedEventArgs(IParticipant participant)
        {
            Participant = participant;
        }
    }
}