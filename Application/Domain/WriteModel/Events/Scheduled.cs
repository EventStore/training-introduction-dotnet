using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Events
{
    public class Scheduled : Event
    {
        public string SlotId { get; }

        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public Scheduled(string slotId, in DateTime startTime, in TimeSpan duration)
        {
            SlotId = slotId;
            StartTime = startTime;
            Duration = duration;
        }

        protected bool Equals(Scheduled other)
        {
            return SlotId.Equals(other.SlotId) && StartTime.Equals(other.StartTime) && Duration.Equals(other.Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Scheduled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SlotId, StartTime, Duration);
        }
    }
}
