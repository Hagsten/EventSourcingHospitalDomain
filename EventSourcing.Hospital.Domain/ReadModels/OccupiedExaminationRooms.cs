using System.Collections.Generic;
using LunchAndLearn.EventSourcing.Domain.Events;

namespace LunchAndLearn.EventSourcing.Domain.ReadModels
{
    public class OccupiedExaminationRooms
    {
        private readonly List<string> _rooms = new();

        public IReadOnlyCollection<string> Rooms => _rooms.AsReadOnly();

        public static OccupiedExaminationRooms Replay(ICollection<object> events)
        {
            var entity = new OccupiedExaminationRooms();

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

        private void Apply(ExaminationStartedEvent e)
        {
            if (_rooms.Contains(e.ExaminationRoom))
            {
                return;
            }

            _rooms.Add(e.ExaminationRoom);
        }

        private void Apply(ExaminationEndedEvent e)
        {
            _rooms.Remove(e.ExaminationRoom);
        }

        private void Apply(object e)
        { }
    }
}
