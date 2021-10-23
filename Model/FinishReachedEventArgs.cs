using System;

namespace Model
{
    public class FinishReachedEventArgs : EventArgs
    {
        public IParticipant Participant;
        
        public FinishReachedEventArgs(IParticipant participant)
        {
            Participant = participant;
        }
    }
}