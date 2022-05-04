using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using Newtonsoft.Json;

namespace EventSourcing.Hospital.Storage
{
    public class Subscription
    {
        private static readonly EventStoreClient Client;
        private static readonly Store Store;

        static Subscription()
        {
            Store = new Store();
            Client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false&keepAliveInterval=-1&keepAliveTimeout=-1"));
        }

        public static async Task StartCatchUpSubscription(string stream, Func<ResolvedEvent, Task> handler)
        {
            var snapshots = await Store.ReadSnapshotStream($"{stream}");

            var position = StreamPosition.Start;

            if (snapshots.Count > 0)
            {
                var meta = JsonConvert.DeserializeObject<SnapshotMetaData>(Encoding.UTF8.GetString(snapshots.Last().Event.Metadata.Span));
                position = new StreamPosition(meta.Position + 1);

                await HandleEvent(snapshots.Last());
            }

            await Client.SubscribeToStreamAsync(stream, position, async (_, evt, _) => await HandleEvent(evt), true);

            async Task HandleEvent(ResolvedEvent evt)
            {
                try
                {
                    await handler(evt);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in subscr. " + e.Message);
                }
            }
        }
    }
}