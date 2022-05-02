using System;

namespace EventSourcing.Hospital.Domain.Events
{
    public class PatientEntryCreatedEvent
    {
        public string PrimaryHospital { get; }
        public string Patient { get; }
        public DateTime Timestamp { get; }

        public PatientEntryCreatedEvent(string primaryHospital, string patient, DateTime timestamp)
        {
            PrimaryHospital = primaryHospital;
            Patient = patient;
            Timestamp = timestamp;
        }
    }
}