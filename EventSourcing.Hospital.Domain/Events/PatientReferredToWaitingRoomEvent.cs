using System;

namespace LunchAndLearn.EventSourcing.Domain.Events
{
    public class PatientReferredToWaitingRoomEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime Timestamp { get; }
        public string WaitingRoom { get; }

        public PatientReferredToWaitingRoomEvent(string hospital, string patient, DateTime timestamp, string waitingRoom)
        {
            Hospital = hospital;
            Patient = patient;
            Timestamp = timestamp;
            WaitingRoom = waitingRoom;
        }
    }
}