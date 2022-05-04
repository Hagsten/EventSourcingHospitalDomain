using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Hospital.Domain;
using EventSourcing.Hospital.Domain.Events;
using EventSourcing.Hospital.Domain.Events.Snapshots;
using EventSourcing.Hospital.Domain.ReadModels;
using EventSourcing.Hospital.Storage;
using EventStore.Client;

namespace EventSourcing.Hospital.App.Snapshots
{
    internal class Program
    {
        private static readonly TotalVisits Model = TotalVisits.Replay(new List<object>());

        private static readonly Store Store;

        static Program()
        {
            Store = new Store();
        }

        static void Main(string[] args)
        {
            StartSubscriptions().GetAwaiter().GetResult();

            //SeedManyAndSnapshot(52384, 1500).GetAwaiter().GetResult();

            Console.WriteLine("TotalVisits subscription started, press [Enter] to see its progress or type 'snapshot' to snap!");

            var cmd = Console.ReadLine();

            while (cmd != "q")
            {
                Console.WriteLine(Model.Count);

                if (cmd == "snapshot")
                {
                    Console.WriteLine("This will create a snapshot of TotalVisits, proceed (y)?");

                    var answer = Console.ReadLine();

                    if (answer == "y")
                    {
                        Snapshot(Model).GetAwaiter().GetResult();
                    }
                }

                cmd = Console.ReadLine();
            }
        }
        
        private static async Task SeedManyAndSnapshot(int from, int howMany)
        {
            var iterations = Enumerable.Range(from, howMany).ToArray();
            var i = 0;
            foreach (var iteration in iterations)
            {
                await SeedFullVisit($"Patient-{iteration}");

                i++;

                if (i % 1000 == 0)
                {
                    Console.WriteLine($"Done with {i} visits");
                }
            }
        }

        private static async Task SeedFullVisit(string patient)
        {
            var rnd = new Random();

            const string hospital = "St Johns";
            var examinationRoom = rnd.Next(0, 10);

            var beginning = DateTime.Now.AddDays(-rnd.Next(0, 365));

            var events = new object[]
            {
                new HospitalEntryCreatedEvent(hospital, beginning.AddDays(-10)),
                new PatientEntryCreatedEvent(hospital, patient, beginning),
                new PatientArrivedEvent(hospital, patient, beginning.AddMinutes(1), "Sigge McQuack"),
                new PatientReferredToWaitingRoomEvent(hospital, patient, beginning.AddMinutes(15), "Waiting room 1"),
                new PatientCalledToExaminationRoomEvent(hospital, patient, beginning.AddMinutes(45), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new ExaminationStartedEvent(hospital, patient, beginning.AddMinutes(50), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new ExaminationEndedEvent(hospital, patient, beginning.AddMinutes(60), $"Examination room {examinationRoom}", "Kajsa Anka"),
                new BloodSampleResultReadyEvent(patient, rnd.Next(0, 100) > 70, rnd.Next(0, 100) > 70, beginning.AddMinutes(120)),
                new PatentDiagnosedEvent(hospital, patient, beginning.AddMinutes(180), "Kajsa Anka", "Coronary heart disease"),
                new PatientDepartedEvent(hospital, patient, beginning.AddMinutes(200))
            };

            await Store.AppendEvent($"hospital-{hospital}", events);
        }

        private static async Task StartSubscriptions()
        {
            await Subscription.StartCatchUpSubscription("hospital-St Johns", HandleEvent);
        }

        private static Task HandleEvent(ResolvedEvent e)
        {
            var evt = EventDeserializer.Deserialize(e.Event.Data.Span.ToArray(), e.Event.EventType);

            Model.UpdateWith(evt);

            return Task.CompletedTask;
        }
        
        private static async Task Snapshot(TotalVisits model)
        {
            await Store.Snapshot("hospital-St Johns", new TotalVisitsSnapshotEvent("St Johns", model.Count));
        }
    }
}
