using System;
using System.Collections.Generic;
using EventSourcing.Hospital.Domain.Events;

namespace EventSourcing.Hospital.Domain.Entities
{
    public class PatientEntity
    {
        public string Name { get; private set; }
        public string PrimaryHospital { get; private set; }
        public string Diagnose { get; private set; } = "None";
        public string Location { get; private set; }
        public bool HasHighBloodpressure { get; private set; }
        public bool HasHighCholesterol { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public DateTime PreviousVisit { get; private set; }
        public string AssignedNurse  { get; private set; }

        private PatientEntity() { }
        
        public static PatientEntity Replay(ICollection<object> events)
        {
            var entity = new PatientEntity();

            foreach (var e in events)
            {
                entity.Apply((dynamic)e);
            }

            return entity;
        }

        private void Apply(PatientEntryCreatedEvent e)
        {
            Name = e.Patient;
            LastUpdated = e.Timestamp;
            PrimaryHospital = e.PrimaryHospital;
        }

        private void Apply(PatientArrivedEvent e)
        {
            LastUpdated = e.ArrivedAt;
            PreviousVisit = e.ArrivedAt;
        }

        private void Apply(PatientReferredToWaitingRoomEvent e)
        {
            LastUpdated = e.Timestamp;
            Location = e.WaitingRoom;
        }

        private void Apply(PatientCalledToExaminationRoomEvent e)
        {
            LastUpdated = e.Timestamp;
            Location = e.ExaminationRoom;
            AssignedNurse = e.Nurse;
        }

        private void Apply(ExaminationStartedEvent e)
        {
            LastUpdated = e.Timestamp;
            Location = e.ExaminationRoom;
            AssignedNurse = e.Nurse;
        }

        private void Apply(ExaminationEndedEvent e)
        {
            LastUpdated = e.Timestamp;
            Location = e.Hospital;
        }

        private void Apply(BloodSampleResultReadyEvent e)
        {
            LastUpdated = e.Timestamp;
            HasHighBloodpressure = e.HighBloodPressure;
            HasHighCholesterol = e.HighCholesterol;
        }

        private void Apply(PatentDiagnosedEvent e)
        {
            LastUpdated = e.DiagnosedAt;
            Diagnose = e.Diagnose;
            AssignedNurse = e.Nurse;
        }

        private void Apply(object e)
        {
            //unhandled
        }
    }
}
