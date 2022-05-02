using System;

namespace EventSourcing.Hospital.Domain.Events
{
    public class BloodSampleResultReadyEvent
    {
        public string Patient { get; }
        public bool HighCholesterol { get; }
        public bool HighBloodPressure { get; }
        public DateTime Timestamp { get; }

        public BloodSampleResultReadyEvent(string patient, bool highCholesterol, bool highBloodPressure, DateTime timestamp)
        {
            Patient = patient;
            HighCholesterol = highCholesterol;
            HighBloodPressure = highBloodPressure;
            Timestamp = timestamp;
        }
    }
}