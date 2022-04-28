using System;
using System.Linq;
using System.Threading.Tasks;
using LunchAndLearn.EventSourcing.Domain;
using LunchAndLearn.EventSourcing.Domain.Entities;
using LunchAndLearn.EventSourcing.Domain.ReadModels;
using LunchAndLearn.EventSourcing.Storage;

namespace LunchAndLearn.EventSourcing.App.ReadPatient
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
            Console.WriteLine("================== Read a patients state ===================");
            Console.WriteLine("What is the patients name?");
            
            var command = Console.ReadLine();

            while (command != "q")
            {
                ReadSingle(command).GetAwaiter().GetResult();

                Console.WriteLine($"{Environment.NewLine}What is the patients name?");
                command = Console.ReadLine();
            }
        }

        private static async Task ReadSingle(string patientName)
        {
            var events = await Store.ReadFromStream($"patient-{patientName}");

            if (events.Count == 0)
            {
                return;
            }
            
            var deserialized = events.Select(resolvedEvent => EventDeserializer.Deserialize(resolvedEvent.Event.Data.Span.ToArray(), resolvedEvent.Event.EventType)).ToList();

            var patient = PatientEntity.Replay(deserialized);
            
            Console.WriteLine($"Patient {patient.Name}'s current state");
            Console.WriteLine($"Primary hospital {patient.PrimaryHospital}");
            Console.WriteLine($"Assigned nurse: {patient.AssignedNurse}");
            Console.WriteLine($"Currently located at: {patient.Location}");
            Console.WriteLine($"Last signed in: {patient.PreviousVisit.ToLongDateString() + ", " + patient.PreviousVisit.ToLongTimeString()}");
            Console.WriteLine($"Diagnose: {patient.Diagnose}");
            Console.WriteLine($"High blood pressure: {patient.HasHighBloodpressure}");
            Console.WriteLine($"High cholesterol: {patient.HasHighCholesterol}");
            
            Console.ReadLine();

            Console.WriteLine($"Time spent in hospital is {TimeAtHospital.Replay(deserialized).Value.ToString()}");            
            
        }
        
        private static async Task ReadModelsFromAll()
        {
            var events = await Store.ReadFromStream("$ce-patient");

            if (events.Count == 0)
            {
                return;
            }
            
            var deserialized = events.Select(resolvedEvent => EventDeserializer.Deserialize(resolvedEvent.Event.Data.Span.ToArray(), resolvedEvent.Event.EventType)).ToList();

            Console.WriteLine($"Occupied rooms are {string.Join(", ", OccupiedExaminationRooms.Replay(deserialized).Rooms)}");
            Console.WriteLine($"Total visits are {TotalVisits.Replay(deserialized).Count}");
        }
    }
}
