using EventSourcing.Hospital.Storage;

namespace EventSourcing.Hospital.Domain.Events.Snapshots
{
    public class TotalVisitsSnapshotEvent : ISnapshotEvent
    {
        public string Hospital { get; }
        public int TotalVisits { get; }

        public TotalVisitsSnapshotEvent(string hospital, int totalVisits)
        {
            Hospital = hospital;
            TotalVisits = totalVisits;
        }
    }
}