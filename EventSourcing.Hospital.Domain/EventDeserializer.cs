using System;
using System.Text;
using EventSourcing.Hospital.Domain.Events;
using Newtonsoft.Json;

namespace EventSourcing.Hospital.Domain
{
    public static class EventDeserializer
    {
        public static object Deserialize(byte[] e, string eventType)
        {
            return eventType switch
            {
                nameof(PatientEntryCreatedEvent) => JsonConvert.DeserializeObject<PatientEntryCreatedEvent>(Encoding.UTF8.GetString(e)),
                nameof(PatientArrivedEvent) => JsonConvert.DeserializeObject<PatientArrivedEvent>(Encoding.UTF8.GetString(e)),
                nameof(PatientReferredToWaitingRoomEvent) => JsonConvert.DeserializeObject<PatientReferredToWaitingRoomEvent>(Encoding.UTF8.GetString(e)),
                nameof(PatientCalledToExaminationRoomEvent) => JsonConvert.DeserializeObject<PatientCalledToExaminationRoomEvent>(Encoding.UTF8.GetString(e)),
                nameof(ExaminationStartedEvent) => JsonConvert.DeserializeObject<ExaminationStartedEvent>(Encoding.UTF8.GetString(e)),
                nameof(BloodSampleResultReadyEvent) => JsonConvert.DeserializeObject<BloodSampleResultReadyEvent>(Encoding.UTF8.GetString(e)),
                nameof(PatentDiagnosedEvent) => JsonConvert.DeserializeObject<PatentDiagnosedEvent>(Encoding.UTF8.GetString(e)),
                nameof(ExaminationEndedEvent) => JsonConvert.DeserializeObject<ExaminationEndedEvent>(Encoding.UTF8.GetString(e)),
                nameof(HospitalEntryCreatedEvent) => JsonConvert.DeserializeObject<HospitalEntryCreatedEvent>(Encoding.UTF8.GetString(e)),
                nameof(PatientDepartedEvent) => JsonConvert.DeserializeObject<PatientDepartedEvent>(Encoding.UTF8.GetString(e)),

                _ =>  throw new NotImplementedException()
            };
        }
    }
}