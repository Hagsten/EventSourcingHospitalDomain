using System.Collections.Generic;
using EventSourcing.Hospital.Domain.Events;
using EventSourcing.Hospital.Domain.Events.Snapshots;

namespace EventSourcing.Hospital.Domain.ReadModels
{
    public class TotalVisits
    {
        public int Count { get; private set; }

        public static TotalVisits Replay(ICollection<object> events)
        {
            var entity = new TotalVisits();

            foreach (var e in events)
            {
                entity.Apply((dynamic)e);
            }

            return entity;
        }


        public void UpdateWith(object evt)
        {
            Apply((dynamic)evt);
        }

        private void Apply(PatientArrivedEvent e)
        {
            Count++;
        }

        private void Apply(TotalVisitsSnapshotEvent e)
        {
            Count = e.TotalVisits;
        }

        private void Apply(object e)
        { }
    }
}