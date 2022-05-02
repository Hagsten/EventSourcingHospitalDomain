using System;

namespace EventSourcing.Hospital.Domain.Events
{
    public class ExaminationStartedEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime Timestamp { get; }
        public string ExaminationRoom { get; }
        public string Nurse { get; }

        public ExaminationStartedEvent(string hospital, string patient, DateTime timestamp, string examinationRoom, string nurse)
        {
            Hospital = hospital;
            Patient = patient;
            Timestamp = timestamp;
            ExaminationRoom = examinationRoom;
            Nurse = nurse;
        }
    }
}