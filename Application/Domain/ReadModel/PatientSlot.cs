using System;

namespace Application.Domain.ReadModel;

public class PatientSlot
{
    public string ScheduledId  { get; }
    public DateTime StartTime { get; }
    public TimeSpan Duration { get; }
    public string Status { get; private set; }

    public PatientSlot(string scheduledId, in DateTime startTime, in TimeSpan duration, String status = "booked")
    {
        ScheduledId = scheduledId;
        StartTime = startTime;
        Duration = duration;
        Status = status;
    }

    private bool Equals(PatientSlot other)
    {
        return ScheduledId.Equals(other.ScheduledId) && StartTime.Equals(other.StartTime) && Duration.Equals(other.Duration) && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((PatientSlot) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ScheduledId, StartTime, Duration, Status);
    }

    public void MarkAsCancelled()
    {
        Status = "cancelled";
    }
}