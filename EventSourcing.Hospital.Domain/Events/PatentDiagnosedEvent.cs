using System;

namespace LunchAndLearn.EventSourcing.Domain.Events
{
    public class PatentDiagnosedEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime DiagnosedAt { get; }
        public string Nurse { get; }
        public string Diagnose { get; }

        public PatentDiagnosedEvent(string hospital, string patient, DateTime diagnosedAt, string nurse, string diagnose)
        {
            Hospital = hospital;
            Patient = patient;
            DiagnosedAt = diagnosedAt;
            Nurse = nurse;
            Diagnose = diagnose;
        }
    }
}