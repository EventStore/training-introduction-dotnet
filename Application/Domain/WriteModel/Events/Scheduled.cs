using System;
using Application.EventSourcing;

namespace Application.Domain.Events
{
    public class Scheduled : Event
    {
        public string Id { get; }
        
        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public Scheduled(string id, in DateTime startTime, in TimeSpan duration)
        {
            Id = id;
            StartTime = startTime;
            Duration = duration;
        }

        protected bool Equals(Scheduled other)
        {
            return Id.Equals(other.Id) && StartTime.Equals(other.StartTime) && Duration.Equals(other.Duration);
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
            return HashCode.Combine(Id, StartTime, Duration);
        }
    }
}