using System;
using System.Collections.Generic;
using LunchAndLearn.EventSourcing.Domain.Events;

namespace LunchAndLearn.EventSourcing.Domain.ReadModels
{
    public class TimeAtHospital
    {
        private DateTime? _arrivedAt;
        private DateTime? _departedAt;

        public TimeSpan Value => _arrivedAt.HasValue ? (_departedAt ?? DateTime.Now) - _arrivedAt.Value : TimeSpan.Zero;

        public static TimeAtHospital Replay(ICollection<object> events)
        {
            var entity = new TimeAtHospital();

            foreach (var e in events)
            {
                entity.Apply((dynamic)e);
            }

            return entity;
        }

        private void Apply(PatientArrivedEvent e)
        {
            _arrivedAt = e.ArrivedAt;
        }

        private void Apply(PatientDepartedEvent e)
        {
            _departedAt = e.DepartedAt;
        }

        private void Apply(object e)
        { }
    }
}