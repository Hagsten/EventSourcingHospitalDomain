using System;

namespace LunchAndLearn.EventSourcing.Domain.Events
{
    public class PatientArrivedEvent
    {
        public string Hospital { get; }
        public string Patient { get; }
        public DateTime ArrivedAt { get; }
        public string AdministredBy { get; }

        public PatientArrivedEvent(string hospital, string patient, DateTime arrivedAt, string administredBy)
        {
            Hospital = hospital;
            Patient = patient;
            ArrivedAt = arrivedAt;
            AdministredBy = administredBy;
        }
    }
}
