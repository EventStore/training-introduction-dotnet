using System;

namespace Application.Domain.ReadModel
{
    public class AvailableSlot
    {
        public string ScheduledId { get; }
        public DateTime ScheduledStartTime { get; }
        public TimeSpan ScheduledDuration { get; }

        public AvailableSlot(string scheduledId, in DateTime scheduledStartTime, in TimeSpan scheduledDuration)
        {
            ScheduledId = scheduledId;
            ScheduledStartTime = scheduledStartTime;
            ScheduledDuration = scheduledDuration;
        }

        protected bool Equals(AvailableSlot other)
        {
            return ScheduledId.Equals(other.ScheduledId) && ScheduledStartTime.Equals(other.ScheduledStartTime) && ScheduledDuration.Equals(other.ScheduledDuration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AvailableSlot) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ScheduledId, ScheduledStartTime, ScheduledDuration);
        }
    }
}