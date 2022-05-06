using System.Collections.Generic;

namespace EventSourcing.Hospital.Domain.ReadModels
{
    //Live code event handlers
    public class PatientsWithHighBloodPressure
    {
        public int Count { get; private set; }
        private int Total;
        public decimal PercentageOfTotal  => Total > 0 ? (Count / (decimal)Total) * 100M : 0;

        private PatientsWithHighBloodPressure()
        {}

        public static PatientsWithHighBloodPressure Replay(ICollection<object> events)
        {
            var entity = new PatientsWithHighBloodPressure();

            foreach (var e in events)
            {
                entity.Apply((dynamic)e);
            }

            return entity;
        }
        
        public void UpdateWith(object evt) => Apply((dynamic)evt);

        //TODO: Event Handlers

        private void Apply(object e)
        { }
    }
}
