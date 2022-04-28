using System;

namespace LunchAndLearn.EventSourcing.Domain.Events
{
    public class PatientDepartedEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime DepartedAt { get; }

        public PatientDepartedEvent(string hospital, string patient, DateTime departedAt)
        {
            Hospital = hospital;
            Patient = patient;
            DepartedAt = departedAt;
        }
    }
}