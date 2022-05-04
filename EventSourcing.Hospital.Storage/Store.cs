using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using Newtonsoft.Json;

namespace EventSourcing.Hospital.Storage
{
    public class Store
    {
        private readonly EventStoreClient _client;

        public Store()
        {
            _client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false"));
        }

        public async Task AppendEvent(string stream, object[] events)
        {
            var eventData = events.Select(e => new EventData(
                    Uuid.NewUuid(),
                    e.GetType().Name,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e))))
                .ToArray();

            await _client.AppendToStreamAsync(
                stream,
                StreamState.Any,
                eventData, o =>
                {
                    o.TimeoutAfter = TimeSpan.FromMinutes(5);
                });
            
        }

        public async Task Snapshot(string stream, ISnapshotEvent snapshot)
        {
            var state = await _client.ReadStreamAsync(
                Direction.Backwards,
                $"{stream}",
                StreamPosition.End,
                1).ToListAsync();

            if (state.Count == 0)
            {
                Console.WriteLine("No need for a snapshot yet...");
                return;
            }

            var pos = state.Single().Event.EventNumber.ToUInt64();

            var eventData = new EventData(
                Uuid.NewUuid(),
                snapshot.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot)),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new SnapshotMetaData(pos))));

            await _client.AppendToStreamAsync(
                $"{stream}-snapshot",
                StreamState.Any,
                new[] { eventData });
        }

        public async Task<IReadOnlyCollection<ResolvedEvent>> ReadFromStream(string stream)
        {
            var response = new List<ResolvedEvent>();

            var snapshots = await GetSnapshotStream(stream);

            var position = StreamPosition.Start;

            if (snapshots.Count > 0)
            {
                Console.WriteLine($"Using snapshot for stream {stream}");
                var meta = JsonConvert.DeserializeObject<SnapshotMetaData>(Encoding.UTF8.GetString(snapshots.Last().Event.Metadata.Span));
                position = new StreamPosition(meta.Position + 1);
                response.Add(snapshots.Last());
            }

            var state = _client.ReadStreamAsync(Direction.Forwards, stream, position, resolveLinkTos: true);

            if (await state.ReadState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }

            response.AddRange(await state.ToListAsync());

            return response;
        }

        private async Task<IReadOnlyCollection<ResolvedEvent>> GetSnapshotStream(string stream)
        {
            var streamName = $"{stream}-snapshot";

            var state = _client.ReadStreamAsync(
                Direction.Backwards,
                streamName,
                StreamPosition.End,
                1);

            var readState = await state.ReadState;

            if (readState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }

            return await state.ToListAsync();
        }

        public async Task<IReadOnlyCollection<ResolvedEvent>> ReadSnapshotStream(string stream) => await GetSnapshotStream(stream);
    }

    public class SnapshotMetaData
    {
        public SnapshotMetaData(ulong position)
        {
            Position = position;
        }

        public ulong Position { get; }
    }
}
