using System;
using System.Threading.Tasks;
using EventSourcing.Hospital.Domain.Events;
using EventSourcing.Hospital.Storage;

namespace EventSourcing.Hospital.App
{
    class Program
    {
        private static readonly Store Store;

        static Program()
        {
            Store = new Store();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("============================ Add some data ============================");
            Console.WriteLine("What is the patients name?");
            var name = Console.ReadLine()?.Trim();

            Console.WriteLine("Seed with a completed visit (1) or not (2)");

            var option = Console.ReadLine();

            while (option != "q")
            {
                if (option == "1")
                {
                    SeedFullVisit(name).GetAwaiter().GetResult();
                }

                if (option == "2")
                {
                    SeedOngoingExamination(name).GetAwaiter().GetResult();
                }

                Console.WriteLine("What is the patients name?");
                name = Console.ReadLine();

                Console.WriteLine("Seed with a completed visit (1) or not (2)");
                option = Console.ReadLine();
            }
        }

        private static async Task SeedOngoingExamination(string patient)
        {
            var rnd = new Random();
            var hospital = "St Johns";

            var examinationRoom = rnd.Next(0, 10);
            var beginning = DateTime.Now.AddDays(-1);

            var events = new object[]
            {
                new PatientEntryCreatedEvent(hospital, patient, beginning),
                new PatientArrivedEvent(hospital, patient, beginning.AddMinutes(1), "Sigge McQuack"),
                new PatientReferredToWaitingRoomEvent(hospital, patient, beginning.AddMinutes(15), "Waiting room 1"),
                new PatientCalledToExaminationRoomEvent(hospital, patient, beginning.AddMinutes(45), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new ExaminationStartedEvent(hospital, patient, beginning.AddMinutes(50), $"Examination room {examinationRoom}", "Kajsa Anka")
            };

            await Store.AppendEvent($"patient-{patient}", events);
        }

        private static async Task SeedFullVisit(string patient)
        {
            var rnd = new Random();

            var hospital = "St Johns";
            var examinationRoom = rnd.Next(0, 10);
            
            var beginning = DateTime.Now.AddDays(-1);

            var events = new object[]
            {
                new HospitalEntryCreatedEvent(hospital, beginning.AddDays(-10)),
                new PatientEntryCreatedEvent(hospital, patient, beginning),
                new PatientArrivedEvent(hospital, patient, beginning.AddMinutes(1), "Sigge McQuack"),
                new PatientReferredToWaitingRoomEvent(hospital, patient, beginning.AddMinutes(15), "Waiting room 1"),
                new PatientCalledToExaminationRoomEvent(hospital, patient, beginning.AddMinutes(45), "Examination room 3", "Kajsa Anka"),
                new ExaminationStartedEvent(hospital, patient, beginning.AddMinutes(50), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new ExaminationEndedEvent(hospital, patient, beginning.AddMinutes(60), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new BloodSampleResultReadyEvent(patient, rnd.Next(0, 100) > 70, rnd.Next(0, 100) > 70, beginning.AddMinutes(120)),
                new PatentDiagnosedEvent(hospital, patient, beginning.AddMinutes(180), "Kajsa Anka", "Coronary heart disease"),
                new PatientDepartedEvent(hospital, patient, beginning.AddMinutes(200))
            };

            await Store.AppendEvent($"patient-{patient}", events);
        }
    }
}
