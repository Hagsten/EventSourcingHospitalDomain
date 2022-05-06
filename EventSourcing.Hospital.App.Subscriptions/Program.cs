using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcing.Hospital.Domain;
using EventSourcing.Hospital.Domain.ReadModels;
using EventSourcing.Hospital.Storage;
using EventStore.Client;

namespace EventSourcing.Hospital.App.Subscriptions
{
    class Program
    {
        private static readonly OccupiedExaminationRooms Model = OccupiedExaminationRooms.Replay(new List<object>());
        private static readonly PatientsWithHighBloodPressure HighBloodPressureModel = PatientsWithHighBloodPressure.Replay(new List<object>());

        static void Main(string[] args)
        {
            Console.WriteLine("=================== Subscriptions ==================");

            StartSubscriptions().GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task StartSubscriptions()
        {
            await Subscription.StartCatchUpSubscription("$ce-patient", HandleEvent);
        }

        private static Task HandleEvent(ResolvedEvent e)
        {
            var evt = EventDeserializer.Deserialize(e.Event.Data.Span.ToArray(), e.Event.EventType);

            UpdateOccupiedExaminationRoomsModel(evt);
            //UpdateBloodPressureModel(evt);

            return Task.CompletedTask;
        }

        private static void UpdateOccupiedExaminationRoomsModel(object evt)
        {
            var rooms = Model.Rooms.Count;

            Model.UpdateWith(evt);

            if (rooms != Model.Rooms.Count)
            {
                Console.WriteLine($"Occupied examination rooms: {Model.Rooms.Count}");
            }
        }

        private static void UpdateBloodPressureModel(object evt)
        {
            HighBloodPressureModel.UpdateWith(evt);

            Console.WriteLine($"Patients with high bloodpressure: {HighBloodPressureModel.Count} ({HighBloodPressureModel.PercentageOfTotal}%)");

        }
    }
}
