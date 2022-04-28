using System;

namespace LunchAndLearn.EventSourcing.Domain.Events
{
    public class PatientCalledToExaminationRoomEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime Timestamp { get; }
        public string ExaminationRoom { get; }
        public string Nurse { get; }

        public PatientCalledToExaminationRoomEvent(string hospital, string patient, DateTime timestamp, string examinationRoom, string nurse)
        {
            Hospital = hospital;
            Patient = patient;
            Timestamp = timestamp;
            ExaminationRoom = examinationRoom;
            Nurse = nurse;
        }
    }
}