using System;

namespace Application.Domain.ReadModel;

public class ScheduledSlot
{
    public string Id { get; }
    public DateTime StartTime { get; }
    public TimeSpan Duration { get; }

    public ScheduledSlot(string id, in DateTime startTime, in TimeSpan duration)
    {
        Id = id;
        StartTime = startTime;
        Duration = duration;
    }

    protected bool Equals(AvailableSlot other)
    {
        return Id.Equals(other.ScheduledId) && StartTime.Equals(other.ScheduledStartTime) && Duration.Equals(other.ScheduledDuration);
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
        return HashCode.Combine(Id, StartTime, Duration);
    }
}